﻿using System.Collections.Generic;
using System.Drawing;
using System.Linq;

using OpenCL.NET.DataTypes;
using OpenCL.NET.Memory;
using OpenCL.Wrapper;

using OpenFL.Core.Buffers;
using OpenFL.Core.Exceptions;
using OpenFL.Core.Instructions.Variables;

using Utility.ADL;

namespace OpenFL.Core.DataObjects.ExecutableDataObjects
{
    public class FLProgram : FLParsedObject
    {

        //public static IDebugger Debugger = null;

        private readonly Stack<FLExecutionContext> ContextStack = new Stack<FLExecutionContext>();
        private readonly List<FLBuffer> internalBuffers = new List<FLBuffer>();
        private MemoryBuffer activeChannelBuffer;
        private byte[] activeChannels;
        private bool channelDirty = true;
        public VariableManager<decimal> Variables = new VariableManager<decimal>();


        private bool warmed;


        public FLProgram(
            CLAPI instance, Dictionary<string, IFunction> definedScripts,
            Dictionary<string, FLBuffer> definedBuffers, Dictionary<string, IFunction> flFunctions)
        {
            Instance = instance;
            FlFunctions = flFunctions;
            DefinedBuffers = definedBuffers;
            DefinedScripts = definedScripts;
            InternalState = new Dictionary<string, bool>();
            foreach (KeyValuePair<string, FLBuffer> definedBuffer in DefinedBuffers)
            {
                InternalState.Add(definedBuffer.Key, true);
            }
        }

        //Get set when calling Run/SetCLVariables
        public CLAPI Instance { get; }

        public FLBuffer ActiveBuffer
        {
            get =>
                DefinedBuffers.ContainsKey(FLKeywords.ActiveBufferKey)
                    ? DefinedBuffers[FLKeywords.ActiveBufferKey]
                    : null;
            set => DefinedBuffers[FLKeywords.ActiveBufferKey] = value;
        }

        public byte[] ActiveChannels
        {
            get => activeChannels;
            set
            {
                if (activeChannels == null || channelDirty)
                {
                    channelDirty = true;
                }
                else
                {
                    for (int i = 0; i < activeChannels.Length; i++)
                    {
                        if (value[i] != activeChannels[i])
                        {
                            channelDirty = true;
                            break;
                        }
                    }

                    if (!channelDirty)
                    {
                        return;
                    }
                }

                activeChannels = value;
            }
        }

        public MemoryBuffer ActiveChannelBuffer
        {
            get
            {
                if (channelDirty)
                {
                    if (activeChannelBuffer == null)
                    {
                        activeChannelBuffer = CLAPI.CreateBuffer(
                                                                 Instance,
                                                                 activeChannels,
                                                                 MemoryFlag.WriteOnly,
                                                                 "ActiveChannelBuffer"
                                                                );
                    }
                    else if (activeChannelBuffer.IsDisposed)
                    {
                        activeChannelBuffer = CLAPI.CreateBuffer(
                                                                 Instance,
                                                                 activeChannels,
                                                                 MemoryFlag.WriteOnly,
                                                                 "ActiveChannelBuffer"
                                                                );
                    }
                    else
                    {
                        CLAPI.WriteToBuffer(Instance, activeChannelBuffer, activeChannels);
                    }

                    channelDirty = false;
                }

                return activeChannelBuffer;
            }
        }

        internal FLBuffer Input { get; set; }

        public int3 Dimensions => new int3(Input.Width, Input.Height, 1);

        public int InputSize => Dimensions.x * Dimensions.y * Dimensions.z * ActiveChannels.Length;

        private Dictionary<string, bool> InternalState { get; }

        public Dictionary<string, FLBuffer> DefinedBuffers { get; }

        public string[] BufferNames => DefinedBuffers.Keys.ToArray();

        public Dictionary<string, IFunction> FlFunctions { get; }

        public Dictionary<string, IFunction> DefinedScripts { get; }

        internal IFunction EntryPoint => FlFunctions.First(x => x.Key == FLKeywords.EntryFunctionKey).Value;

        public bool HasBufferWithName(string name)
        {
            return DefinedBuffers.ContainsKey(name);
        }

        public FLBuffer GetBufferWithName(string name, bool makeUnmanaged)
        {
            FLBuffer ret = DefinedBuffers[name];
            if (makeUnmanaged)
            {
                InternalState[name] = false;
            }

            return ret;
        }

        public Bitmap GetActiveBitmap()
        {
            Bitmap bmp = new Bitmap(Dimensions.x, Dimensions.y);
            CLAPI.UpdateBitmap(Instance, bmp, ActiveBuffer.Buffer);
            return bmp;
        }

        public FLBuffer GetActiveBuffer(bool makeUnmanaged)
        {
            FLBuffer ret = ActiveBuffer;
            if (makeUnmanaged)
            {
                InternalState[ret.DefinedBufferName] = false;
            }

            return ret;
        }

        internal void RemoveFromSystem(FLBuffer buffer)
        {
            if (ActiveBuffer == buffer)
            {
                ActiveBuffer = null;
            }

            if (Input == buffer)
            {
                Input = null;
            }

            internalBuffers.Remove(buffer);

            string[] keys = DefinedBuffers.Where(x => x.Value == buffer).Select(x => x.Key).ToArray();
            for (int i = 0; i < keys.Length; i++)
            {
                DefinedBuffers.Remove(keys[i]);
                InternalState.Remove(keys[i]);
            }
        }

        public void FreeResources()
        {
            foreach (KeyValuePair<string, FLBuffer> definedBuffer in DefinedBuffers)
            {
                if (definedBuffer.Key == FLKeywords.ActiveBufferKey || !InternalState[definedBuffer.Key])
                {
                    continue;
                }

                definedBuffer.Value.Dispose();
            }

            foreach (FLBuffer internalBuffer in internalBuffers)
            {
                internalBuffer.Dispose();
            }

            activeChannelBuffer?.Dispose();
            internalBuffers.Clear();
        }

        public void PushContext()
        {
            ContextStack.Push(new FLExecutionContext(new List<byte>(ActiveChannels).ToArray(), ActiveBuffer));
            ActiveChannels = new byte[] { 1, 1, 1, 1 };
            ActiveBuffer = null;
        }

        public void ReturnFromContext()
        {
            FLExecutionContext context = ContextStack.Pop();
            ActiveChannels = context.ActiveChannels;
            ActiveBuffer = context.ActiveBuffer;
        }

        public FLBuffer RegisterUnmanagedBuffer(FLBuffer buffer)
        {
            internalBuffers.Add(buffer);
            buffer.SetKey(buffer.Buffer.ToString());

            //Debugger?.OnAddInternalBuffer(this, buffer);
            FLDebuggerHelper.OnInternalBufferLoad(
                                                  this,
                                                  new FLDebuggerEvents.InternalBufferLoadEventArgs(buffer.Root, buffer)
                                                 );
            return buffer;
        }


        public void SetCLVariables(FLBuffer input, bool makeInputInternal)
        {
            SetCLVars(input, makeInputInternal);

            WarmBuffers(false);

            warmed = true;
        }

        private void SetCLVars(FLBuffer input, bool makeInputInternal)
        {
            DefinedBuffers[FLKeywords.InputBufferKey]
                .ReplaceUnderlyingBuffer(
                                         input.Buffer,
                                         input.Width,
                                         input.Height,
                                         input.Depth
                                        ); //Making effectively a zombie object that has no own buffer(but this is needed in order to keep the script intact

            //The Arguments that are referencing the IN buffer will otherwise have a different buffer as the input.
            Input = ActiveBuffer = DefinedBuffers[FLKeywords.InputBufferKey];
            InternalState[FLKeywords.InputBufferKey] = makeInputInternal;
        }

        public void SetCLVariablesAndWarm(FLBuffer input, bool makeInputInternal, bool warmBuffers)
        {
            SetCLVars(input, makeInputInternal);


            WarmBuffers(warmBuffers);

            warmed = true;

            Logger.Log(LogType.Log, "Warming Buffers finished", 1);
        }


        private void WarmBuffers(bool force)
        {
            if (!warmed)
            {
                return;
            }

            Logger.Log(LogType.Log, "Warming Buffers...", 1);
            foreach (KeyValuePair<string, FLBuffer> definedBuffer in DefinedBuffers)
            {
                if (definedBuffer.Value is IWarmable warmable)
                {
                    FLDebuggerHelper.OnBufferWarm(this, new FLDebuggerEvents.WarmEventArgs(this, warmable));

                    //Debugger?.ProcessEvent(definedBuffer.Value);
                    warmable.Warm(force);
                }
            }
        }

        public void Run(FLBuffer input, bool makeInputInternal, FLFunction entry = null, bool warmBuffers = false)
        {
            IFunction entryPoint = entry ?? (FlFunctions.Any(x=>x.Key==FLKeywords.EntryFunctionKey)? EntryPoint: throw new FLInvalidEntryPointException("'Main' was not Found"));
            if (entryPoint.Name == FLKeywords.EntryFunctionKey)
            {
                //Debugger?.ProgramStart(this);
                FLDebuggerHelper.OnProgramStart(
                                                this,
                                                new FLDebuggerEvents.ProgramStartEventArgs(
                                                                                           this,
                                                                                           entryPoint,
                                                                                           warmBuffers
                                                                                          )
                                               );
            }

            if (!warmed)
            {
                SetCLVariablesAndWarm(input, makeInputInternal, warmBuffers);
            }

            Input.SetKey(FLKeywords.InputBufferKey);

            //Start Setup
            ActiveChannels = new byte[] { 1, 1, 1, 1 };

            entryPoint.Process();

            warmed = false;

            if (entryPoint.Name == FLKeywords.EntryFunctionKey)
            {
                //Debugger?.ProgramExit(this);
                FLDebuggerHelper.OnProgramExit(this, new FLDebuggerEvents.ProgramExitEventArgs(this));
            }
        }

    }
}
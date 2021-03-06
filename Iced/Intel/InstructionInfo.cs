﻿/*
    Copyright (C) 2018 de4dot@gmail.com

    This file is part of Iced.

    Iced is free software: you can redistribute it and/or modify
    it under the terms of the GNU Lesser General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    Iced is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU Lesser General Public License for more details.

    You should have received a copy of the GNU Lesser General Public License
    along with Iced.  If not, see <https://www.gnu.org/licenses/>.
*/

#if !NO_INSTR_INFO
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Iced.Intel.InstructionInfoInternal;

namespace Iced.Intel {
	/// <summary>
	/// Contains information about an instruction, eg. read/written registers, read/written RFLAGS bits, CPUID feature bit, etc
	/// </summary>
	public readonly struct InstructionInfo {
		readonly UsedRegister[] usedRegisters;
		readonly UsedMemory[] usedMemoryLocations;
		readonly ushort usedRegistersLength;
		readonly ushort usedMemoryLocationsLength;
		readonly ushort opMaskFlags;
		readonly byte cpuidFeature;
		readonly byte flowControl;
		readonly byte encoding;
		readonly byte rflagsInfo;
		readonly byte flags;

		[Flags]
		internal enum OpMaskFlags : ushort {
			OpAccessMask			= 7,
			Op1AccessShift			= 3,
			Op2AccessShift			= 6,
			Op3AccessShift			= 9,
		}

		[Flags]
		enum Flags : byte {
			SaveRestore				= 0x01,
			StackInstruction		= 0x02,
			ProtectedMode			= 0x04,
			Privileged				= 0x08,
		}

		internal InstructionInfo(ref SimpleList<UsedRegister> usedRegisters, ref SimpleList<UsedMemory> usedMemoryLocations, uint opMaskFlags, RflagsInfo rflagsInfo, uint flags1, uint flags2) {
			Debug.Assert(usedRegisters.Array != null);
			this.usedRegisters = usedRegisters.Array;
			Debug.Assert(usedMemoryLocations.Array != null);
			this.usedMemoryLocations = usedMemoryLocations.Array;
			Debug.Assert((uint)usedRegisters.ValidLength <= ushort.MaxValue);
			usedRegistersLength = (ushort)usedRegisters.ValidLength;
			Debug.Assert((uint)usedMemoryLocations.ValidLength <= ushort.MaxValue);
			usedMemoryLocationsLength = (ushort)usedMemoryLocations.ValidLength;
			this.opMaskFlags = (ushort)opMaskFlags;
			Debug.Assert(((flags2 >> (int)InfoFlags2.CpuidFeatureShift) & (uint)InfoFlags2.CpuidFeatureMask) <= byte.MaxValue);
			cpuidFeature = (byte)((flags2 >> (int)InfoFlags2.CpuidFeatureShift) & (uint)InfoFlags2.CpuidFeatureMask);
			Debug.Assert(((flags2 >> (int)InfoFlags2.FlowControlShift) & (uint)InfoFlags2.FlowControlMask) <= byte.MaxValue);
			flowControl = (byte)((flags2 >> (int)InfoFlags2.FlowControlShift) & (uint)InfoFlags2.FlowControlMask);
			Debug.Assert(((flags2 >> (int)InfoFlags2.EncodingShift) & (uint)InfoFlags2.EncodingMask) <= byte.MaxValue);
			encoding = (byte)((flags2 >> (int)InfoFlags2.EncodingShift) & (uint)InfoFlags2.EncodingMask);
			Debug.Assert((uint)rflagsInfo <= byte.MaxValue);
			Debug.Assert((uint)rflagsInfo < (uint)RflagsInfo.Last);
			this.rflagsInfo = (byte)rflagsInfo;

			Debug.Assert((uint)InfoFlags1.SaveRestore == 0x08000000);
			Debug.Assert((uint)InfoFlags1.StackInstruction == 0x10000000);
			Debug.Assert((uint)InfoFlags1.ProtectedMode == 0x20000000);
			Debug.Assert((uint)InfoFlags1.Privileged == 0x40000000);
			Debug.Assert((uint)Flags.SaveRestore == 0x01);
			Debug.Assert((uint)Flags.StackInstruction == 0x02);
			Debug.Assert((uint)Flags.ProtectedMode == 0x04);
			Debug.Assert((uint)Flags.Privileged == 0x08);
			// Bit 4 could be set but we don't use it so we don't need to mask it out
			flags = (byte)(flags1 >> 27);
		}

		void ThrowArgumentOutOfRangeException(string paramName) => throw new ArgumentOutOfRangeException(paramName);

		/// <summary>
		/// Gets a struct iterator that returns all read and written registers. There are some exceptions, this method doesn't return all used registers:
		/// 
		/// 1) If <see cref="SaveRestoreInstruction"/> is true, or
		/// 
		/// 2) If it's a <see cref="FlowControl.Call"/> or <see cref="FlowControl.Interrupt"/> instruction (call, sysenter, int n etc), it can read and write any register (including RFLAGS).
		/// </summary>
		/// <returns></returns>
		public UsedRegisterIterator GetUsedRegisters() => new UsedRegisterIterator(usedRegisters, usedRegistersLength);

		/// <summary>
		/// Gets a struct iterator that returns all read and written memory locations
		/// </summary>
		/// <returns></returns>
		public UsedMemoryIterator GetUsedMemory() => new UsedMemoryIterator(usedMemoryLocations, usedMemoryLocationsLength);

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
		public struct UsedRegisterIterator : IEnumerable<UsedRegister>, IEnumerator<UsedRegister> {
			readonly UsedRegister[] usedRegisters;
			readonly uint length;
			int index;

			internal UsedRegisterIterator(UsedRegister[] usedRegisters, uint length) {
				this.usedRegisters = usedRegisters;
				this.length = length;
				index = -1;
			}

			public UsedRegisterIterator GetEnumerator() => this;
			public UsedRegister Current => usedRegisters[index];

			public bool MoveNext() {
				index++;
				return (uint)index < length;
			}

			IEnumerator<UsedRegister> IEnumerable<UsedRegister>.GetEnumerator() => GetEnumerator();
			IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
			UsedRegister IEnumerator<UsedRegister>.Current => Current;
			object IEnumerator.Current => Current;
			bool IEnumerator.MoveNext() => MoveNext();
			void IEnumerator.Reset() => throw new NotSupportedException();
			public void Dispose() { }
		}

		public struct UsedMemoryIterator : IEnumerable<UsedMemory>, IEnumerator<UsedMemory> {
			readonly UsedMemory[] usedMemoryLocations;
			readonly uint length;
			int index;

			internal UsedMemoryIterator(UsedMemory[] usedMemoryLocations, uint length) {
				this.usedMemoryLocations = usedMemoryLocations;
				this.length = length;
				index = -1;
			}

			public UsedMemoryIterator GetEnumerator() => this;
			public UsedMemory Current => usedMemoryLocations[index];

			public bool MoveNext() {
				index++;
				return (uint)index < length;
			}

			IEnumerator<UsedMemory> IEnumerable<UsedMemory>.GetEnumerator() => GetEnumerator();
			IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
			UsedMemory IEnumerator<UsedMemory>.Current => Current;
			object IEnumerator.Current => Current;
			bool IEnumerator.MoveNext() => MoveNext();
			void IEnumerator.Reset() => throw new NotSupportedException();
			public void Dispose() { }
		}
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member

		/// <summary>
		/// true if the instruction isn't available in real mode or virtual 8086 mode
		/// </summary>
		public bool ProtectedMode => (flags & (uint)Flags.ProtectedMode) != 0;

		/// <summary>
		/// true if this is a privileged instruction
		/// </summary>
		public bool Privileged => (flags & (uint)Flags.Privileged) != 0;

		/// <summary>
		/// true if this is an instruction that implicitly uses the stack pointer (SP/ESP/RSP), eg. call, push, pop, ret, etc.
		/// See also <see cref="Instruction.StackPointerIncrement"/>
		/// </summary>
		public bool StackInstruction => (flags & (uint)Flags.StackInstruction) != 0;

		/// <summary>
		/// true if it's an instruction that saves or restores too many registers (eg. fxrstor, xsave, etc).
		/// <see cref="GetUsedRegisters"/> won't return all read/written registers.
		/// </summary>
		public bool SaveRestoreInstruction => (flags & (uint)Flags.SaveRestore) != 0;

		/// <summary>
		/// Instruction encoding, eg. legacy, VEX, EVEX, ...
		/// </summary>
		public EncodingKind Encoding => (EncodingKind)encoding;

		/// <summary>
		/// CPU or CPUID feature flag
		/// </summary>
		public CpuidFeature CpuidFeature => (CpuidFeature)cpuidFeature;

		/// <summary>
		/// Flow control info
		/// </summary>
		public FlowControl FlowControl => (FlowControl)flowControl;

		/// <summary>
		/// Operand #0 access
		/// </summary>
		public OpAccess Op0Access => (OpAccess)(opMaskFlags & (uint)OpMaskFlags.OpAccessMask);

		/// <summary>
		/// Operand #1 access
		/// </summary>
		public OpAccess Op1Access => (OpAccess)(((uint)opMaskFlags >> (int)OpMaskFlags.Op1AccessShift) & (uint)OpMaskFlags.OpAccessMask);

		/// <summary>
		/// Operand #2 access
		/// </summary>
		public OpAccess Op2Access => (OpAccess)(((uint)opMaskFlags >> (int)OpMaskFlags.Op2AccessShift) & (uint)OpMaskFlags.OpAccessMask);

		/// <summary>
		/// Operand #3 access
		/// </summary>
		public OpAccess Op3Access => (OpAccess)(((uint)opMaskFlags >> (int)OpMaskFlags.Op3AccessShift) & (uint)OpMaskFlags.OpAccessMask);

		/// <summary>
		/// Gets operand access
		/// </summary>
		/// <param name="operand">Operand number, 0-3</param>
		/// <returns></returns>
		public OpAccess GetOpAccess(int operand) {
			switch (operand) {
			case 0: return Op0Access;
			case 1: return Op1Access;
			case 2: return Op2Access;
			case 3: return Op3Access;
			default:
				ThrowArgumentOutOfRangeException(nameof(operand));
				return 0;
			}
		}

		/// <summary>
		/// All flags that are read by the CPU when executing the instruction
		/// </summary>
		public RflagsBits RflagsRead => (RflagsBits)RflagsInfoConstants.flagsRead[rflagsInfo];

		/// <summary>
		/// All flags that are written by the CPU, except those flags that are known to be undefined, always set or always cleared. See also <see cref="RflagsModified"/>
		/// </summary>
		public RflagsBits RflagsWritten => (RflagsBits)RflagsInfoConstants.flagsWritten[rflagsInfo];

		/// <summary>
		/// All flags that are always cleared by the CPU
		/// </summary>
		public RflagsBits RflagsCleared => (RflagsBits)RflagsInfoConstants.flagsCleared[rflagsInfo];

		/// <summary>
		/// All flags that are always set by the CPU
		/// </summary>
		public RflagsBits RflagsSet => (RflagsBits)RflagsInfoConstants.flagsSet[rflagsInfo];

		/// <summary>
		/// All flags that are undefined after executing the instruction
		/// </summary>
		public RflagsBits RflagsUndefined => (RflagsBits)RflagsInfoConstants.flagsUndefined[rflagsInfo];

		/// <summary>
		/// All flags that are modified by the CPU. This is <see cref="RflagsWritten"/> + <see cref="RflagsCleared"/> + <see cref="RflagsSet"/> + <see cref="RflagsUndefined"/>
		/// </summary>
		public RflagsBits RflagsModified => (RflagsBits)RflagsInfoConstants.flagsModified[rflagsInfo];
	}
}
#endif

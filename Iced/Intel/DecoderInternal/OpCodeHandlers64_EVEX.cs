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

#if !NO_DECODER64 && !NO_DECODER
using System.Diagnostics;

namespace Iced.Intel.DecoderInternal.OpCodeHandlers64 {
	sealed class OpCodeHandler_EVEX : OpCodeHandlerModRM {
		public override void Decode(Decoder decoder, ref Instruction instruction) => decoder.EVEX_MVEX(ref instruction);
	}

	sealed class OpCodeHandler_EVEX_V_H_Ev_er : OpCodeHandlerModRM {
		readonly Register baseReg;
		readonly Code codeW0;
		readonly Code codeW1;
		readonly TupleType tupleType;
		readonly MemorySize memSizeW0;
		readonly MemorySize memSizeW1;
		readonly bool onlySAE;
		readonly bool noERd;

		public OpCodeHandler_EVEX_V_H_Ev_er(Register baseReg, Code codeW0, Code codeW1, TupleType tupleType, MemorySize memSizeW0, MemorySize memSizeW1, bool onlySAE, bool noERd) {
			this.baseReg = baseReg;
			this.codeW0 = codeW0;
			this.codeW1 = codeW1;
			this.tupleType = tupleType;
			this.memSizeW0 = memSizeW0;
			this.memSizeW1 = memSizeW1;
			this.onlySAE = onlySAE;
			this.noERd = noERd;
		}

		public OpCodeHandler_EVEX_V_H_Ev_er(Register baseReg, Code codeW0, Code codeW1, TupleType tupleType, MemorySize memSizeW0, MemorySize memSizeW1, bool onlySAE) {
			this.baseReg = baseReg;
			this.codeW0 = codeW0;
			this.codeW1 = codeW1;
			this.tupleType = tupleType;
			this.memSizeW0 = memSizeW0;
			this.memSizeW1 = memSizeW1;
			this.onlySAE = onlySAE;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.EVEX);
			if ((state.flags & StateFlags.W) != 0)
				instruction.InternalCode = codeW1;
			else
				instruction.InternalCode = codeW0;
			instruction.InternalOpCount = 3;
			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp0Kind = OpKind.Register;
			instruction.Op0Register = (int)(state.reg + state.extraRegisterBase + state.extraRegisterBaseEVEX) + baseReg;
			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp1Kind = OpKind.Register;
			instruction.Op1Register = (int)state.vvvv + baseReg;
			if (state.mod == 3) {
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp2Kind = OpKind.Register;
				instruction.Op2Register = (int)(state.rm + state.extraBaseRegisterBase + state.extraBaseRegisterBaseEVEX) + ((state.flags & StateFlags.W) != 0 ? Register.RAX : Register.EAX);
				if ((state.flags & StateFlags.b) != 0 && (!noERd || (state.flags & StateFlags.W) != 0)) {
					if (onlySAE)
						instruction.InternalSetSuppressAllExceptions();
					else {
						Debug.Assert((int)RoundingControl.None == 0);
						Debug.Assert((int)RoundingControl.RoundToNearest == 1);
						Debug.Assert((int)RoundingControl.RoundDown == 2);
						Debug.Assert((int)RoundingControl.RoundUp == 3);
						Debug.Assert((int)RoundingControl.RoundTowardZero == 4);
						instruction.InternalRoundingControl = (uint)state.vectorLength + 1;
					}
				}
			}
			else {
				instruction.InternalOp3Kind = OpKind.Memory;
				if ((state.flags & StateFlags.W) != 0)
					instruction.InternalMemorySize = memSizeW1;
				else
					instruction.InternalMemorySize = memSizeW0;
				decoder.ReadOpMem_m64(ref instruction, tupleType);
			}
		}
	}

	sealed class OpCodeHandler_EVEX_V_H_Ev_Ib : OpCodeHandlerModRM {
		readonly Register baseReg;
		readonly Code codeW0;
		readonly Code codeW1;
		readonly TupleType tupleTypeW0;
		readonly TupleType tupleTypeW1;
		readonly MemorySize memSizeW0;
		readonly MemorySize memSizeW1;

		public OpCodeHandler_EVEX_V_H_Ev_Ib(Register baseReg, Code codeW0, Code codeW1, TupleType tupleTypeW0, TupleType tupleTypeW1, MemorySize memSizeW0, MemorySize memSizeW1) {
			this.baseReg = baseReg;
			this.codeW0 = codeW0;
			this.codeW1 = codeW1;
			this.tupleTypeW0 = tupleTypeW0;
			this.tupleTypeW1 = tupleTypeW1;
			this.memSizeW0 = memSizeW0;
			this.memSizeW1 = memSizeW1;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.EVEX);
			if ((state.flags & StateFlags.W) != 0)
				instruction.InternalCode = codeW1;
			else
				instruction.InternalCode = codeW0;
			instruction.InternalOpCount = 4;
			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp0Kind = OpKind.Register;
			instruction.Op0Register = (int)(state.reg + state.extraRegisterBase + state.extraRegisterBaseEVEX) + baseReg;
			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp1Kind = OpKind.Register;
			instruction.Op1Register = (int)state.vvvv + baseReg;
			if (state.mod == 3) {
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp2Kind = OpKind.Register;
				instruction.Op2Register = (int)(state.rm + state.extraBaseRegisterBase + state.extraBaseRegisterBaseEVEX) + ((state.flags & StateFlags.W) != 0 ? Register.RAX : Register.EAX);
			}
			else {
				instruction.InternalOp3Kind = OpKind.Memory;
				if ((state.flags & StateFlags.W) != 0) {
					instruction.InternalMemorySize = memSizeW1;
					decoder.ReadOpMem_m64(ref instruction, tupleTypeW1);
				}
				else {
					instruction.InternalMemorySize = memSizeW0;
					decoder.ReadOpMem_m64(ref instruction, tupleTypeW0);
				}
			}
			instruction.InternalOp4Kind = OpKind.Immediate8;
			instruction.InternalImmediate8 = decoder.ReadByte();
		}
	}

	sealed class OpCodeHandler_EVEX_Ed_V_Ib : OpCodeHandlerModRM {
		readonly Register baseReg;
		readonly Code code32;
		readonly Code code64;
		readonly TupleType tupleType32;
		readonly TupleType tupleType64;
		readonly MemorySize memSize32;
		readonly MemorySize memSize64;

		public OpCodeHandler_EVEX_Ed_V_Ib(Register baseReg, Code code32, Code code64, TupleType tupleType32, TupleType tupleType64, MemorySize memSize32, MemorySize memSize64) {
			this.baseReg = baseReg;
			this.code32 = code32;
			this.code64 = code64;
			this.tupleType32 = tupleType32;
			this.tupleType64 = tupleType64;
			this.memSize32 = memSize32;
			this.memSize64 = memSize64;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.EVEX);
			if ((int)state.vvvv != 0) {
				decoder.SetInvalidInstruction();
				return;
			}
			Register gpr;
			if ((state.flags & StateFlags.W) != 0) {
				instruction.InternalCode = code64;
				gpr = Register.RAX;
			}
			else {
				instruction.InternalCode = code32;
				gpr = Register.EAX;
			}
			instruction.InternalOpCount = 3;
			if (state.mod == 3) {
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp0Kind = OpKind.Register;
				instruction.Op0Register = (int)(state.rm + state.extraBaseRegisterBase) + gpr;
			}
			else {
				instruction.InternalOp1Kind = OpKind.Memory;
				if ((state.flags & StateFlags.W) != 0) {
					instruction.InternalMemorySize = memSize64;
					decoder.ReadOpMem_m64(ref instruction, tupleType64);
				}
				else {
					instruction.InternalMemorySize = memSize32;
					decoder.ReadOpMem_m64(ref instruction, tupleType32);
				}
			}
			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp1Kind = OpKind.Register;
			instruction.Op1Register = (int)(state.reg + state.extraRegisterBase + state.extraRegisterBaseEVEX) + baseReg;
			instruction.InternalOp3Kind = OpKind.Immediate8;
			instruction.InternalImmediate8 = decoder.ReadByte();
		}
	}

	sealed class OpCodeHandler_EVEX_VkHW_er : OpCodeHandlerModRM {
		readonly Register baseReg;
		readonly Code code;
		readonly TupleType tupleType;
		readonly MemorySize memSize;
		readonly MemorySize memSizeBroadcast;
		readonly bool onlySAE;

		public OpCodeHandler_EVEX_VkHW_er(Register baseReg, Code code, TupleType tupleType, MemorySize memSize, bool onlySAE) {
			this.baseReg = baseReg;
			this.code = code;
			this.tupleType = tupleType;
			this.memSize = memSize;
			memSizeBroadcast = memSize;
			this.onlySAE = onlySAE;
		}

		public OpCodeHandler_EVEX_VkHW_er(Register baseReg, Code code, TupleType tupleType, MemorySize memSize, MemorySize memSizeBroadcast, bool onlySAE) {
			this.baseReg = baseReg;
			this.code = code;
			this.tupleType = tupleType;
			this.memSize = memSize;
			this.memSizeBroadcast = memSizeBroadcast;
			this.onlySAE = onlySAE;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.EVEX);
			instruction.InternalCode = code;
			instruction.InternalOpCount = 3;

			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp0Kind = OpKind.Register;
			instruction.Op0Register = (int)(state.reg + state.extraRegisterBase + state.extraRegisterBaseEVEX) + baseReg;
			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp1Kind = OpKind.Register;
			instruction.Op1Register = (int)state.vvvv + baseReg;
			if (state.mod == 3) {
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp2Kind = OpKind.Register;
				instruction.Op2Register = (int)(state.rm + state.extraBaseRegisterBase + state.extraBaseRegisterBaseEVEX) + baseReg;
				if ((state.flags & StateFlags.b) != 0) {
					if (onlySAE)
						instruction.InternalSetSuppressAllExceptions();
					else {
						Debug.Assert((int)RoundingControl.None == 0);
						Debug.Assert((int)RoundingControl.RoundToNearest == 1);
						Debug.Assert((int)RoundingControl.RoundDown == 2);
						Debug.Assert((int)RoundingControl.RoundUp == 3);
						Debug.Assert((int)RoundingControl.RoundTowardZero == 4);
						instruction.InternalRoundingControl = (uint)state.vectorLength + 1;
					}
				}
			}
			else {
				instruction.InternalOp3Kind = OpKind.Memory;
				if ((state.flags & StateFlags.b) != 0)
					instruction.InternalMemorySize = memSizeBroadcast;
				else
					instruction.InternalMemorySize = memSize;
				decoder.ReadOpMem_m64(ref instruction, tupleType);
			}
			instruction.InternalOpMask = state.aaa;
			if ((state.flags & StateFlags.z) != 0)
				instruction.InternalSetZeroingMasking();
		}
	}

	sealed class OpCodeHandler_EVEX_VkW_er : OpCodeHandlerModRM {
		readonly Register baseReg1;
		readonly Register baseReg2;
		readonly Code code;
		readonly TupleType tupleType;
		readonly MemorySize memSize;
		readonly MemorySize memSizeBroadcast;
		readonly bool onlySAE;

		public OpCodeHandler_EVEX_VkW_er(Register baseReg, Code code, TupleType tupleType, MemorySize memSize, MemorySize memSizeBroadcast, bool onlySAE) {
			baseReg1 = baseReg;
			baseReg2 = baseReg;
			this.code = code;
			this.tupleType = tupleType;
			this.memSize = memSize;
			this.memSizeBroadcast = memSizeBroadcast;
			this.onlySAE = onlySAE;
		}

		public OpCodeHandler_EVEX_VkW_er(Register baseReg1, Register baseReg2, Code code, TupleType tupleType, MemorySize memSize, bool onlySAE) {
			this.baseReg1 = baseReg1;
			this.baseReg2 = baseReg2;
			this.code = code;
			this.tupleType = tupleType;
			this.memSize = memSize;
			memSizeBroadcast = memSize;
			this.onlySAE = onlySAE;
		}

		public OpCodeHandler_EVEX_VkW_er(Register baseReg1, Register baseReg2, Code code, TupleType tupleType, MemorySize memSize, MemorySize memSizeBroadcast, bool onlySAE) {
			this.baseReg1 = baseReg1;
			this.baseReg2 = baseReg2;
			this.code = code;
			this.tupleType = tupleType;
			this.memSize = memSize;
			this.memSizeBroadcast = memSizeBroadcast;
			this.onlySAE = onlySAE;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.EVEX);
			if ((int)state.vvvv != 0) {
				decoder.SetInvalidInstruction();
				return;
			}
			instruction.InternalCode = code;
			instruction.InternalOpCount = 2;

			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp0Kind = OpKind.Register;
			instruction.Op0Register = (int)(state.reg + state.extraRegisterBase + state.extraRegisterBaseEVEX) + baseReg1;
			if (state.mod == 3) {
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp1Kind = OpKind.Register;
				instruction.Op1Register = (int)(state.rm + state.extraBaseRegisterBase + state.extraBaseRegisterBaseEVEX) + baseReg2;
				if ((state.flags & StateFlags.b) != 0) {
					if (onlySAE)
						instruction.InternalSetSuppressAllExceptions();
					else {
						Debug.Assert((int)RoundingControl.None == 0);
						Debug.Assert((int)RoundingControl.RoundToNearest == 1);
						Debug.Assert((int)RoundingControl.RoundDown == 2);
						Debug.Assert((int)RoundingControl.RoundUp == 3);
						Debug.Assert((int)RoundingControl.RoundTowardZero == 4);
						instruction.InternalRoundingControl = (uint)state.vectorLength + 1;
					}
				}
			}
			else {
				instruction.InternalOp2Kind = OpKind.Memory;
				if ((state.flags & StateFlags.b) != 0)
					instruction.InternalMemorySize = memSizeBroadcast;
				else
					instruction.InternalMemorySize = memSize;
				decoder.ReadOpMem_m64(ref instruction, tupleType);
			}
			instruction.InternalOpMask = state.aaa;
			if ((state.flags & StateFlags.z) != 0)
				instruction.InternalSetZeroingMasking();
		}
	}

	sealed class OpCodeHandler_EVEX_VkWIb_er : OpCodeHandlerModRM {
		readonly Register baseReg1;
		readonly Register baseReg2;
		readonly Code code;
		readonly TupleType tupleType;
		readonly MemorySize memSize;
		readonly MemorySize memSizeBroadcast;
		readonly bool onlySAE;

		public OpCodeHandler_EVEX_VkWIb_er(Register baseReg, Code code, TupleType tupleType, MemorySize memSize, bool onlySAE) {
			baseReg1 = baseReg;
			baseReg2 = baseReg;
			this.code = code;
			this.tupleType = tupleType;
			this.memSize = memSize;
			memSizeBroadcast = memSize;
			this.onlySAE = onlySAE;
		}

		public OpCodeHandler_EVEX_VkWIb_er(Register baseReg, Code code, TupleType tupleType, MemorySize memSize, MemorySize memSizeBroadcast, bool onlySAE) {
			baseReg1 = baseReg;
			baseReg2 = baseReg;
			this.code = code;
			this.tupleType = tupleType;
			this.memSize = memSize;
			this.memSizeBroadcast = memSizeBroadcast;
			this.onlySAE = onlySAE;
		}

		public OpCodeHandler_EVEX_VkWIb_er(Register baseReg1, Register baseReg2, Code code, TupleType tupleType, MemorySize memSize, bool onlySAE) {
			this.baseReg1 = baseReg1;
			this.baseReg2 = baseReg2;
			this.code = code;
			this.tupleType = tupleType;
			this.memSize = memSize;
			memSizeBroadcast = memSize;
			this.onlySAE = onlySAE;
		}

		public OpCodeHandler_EVEX_VkWIb_er(Register baseReg1, Register baseReg2, Code code, TupleType tupleType, MemorySize memSize, MemorySize memSizeBroadcast, bool onlySAE) {
			this.baseReg1 = baseReg1;
			this.baseReg2 = baseReg2;
			this.code = code;
			this.tupleType = tupleType;
			this.memSize = memSize;
			this.memSizeBroadcast = memSizeBroadcast;
			this.onlySAE = onlySAE;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.EVEX);
			if ((int)state.vvvv != 0) {
				decoder.SetInvalidInstruction();
				return;
			}
			instruction.InternalCode = code;
			instruction.InternalOpCount = 3;

			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp0Kind = OpKind.Register;
			instruction.Op0Register = (int)(state.reg + state.extraRegisterBase + state.extraRegisterBaseEVEX) + baseReg1;
			if (state.mod == 3) {
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp1Kind = OpKind.Register;
				instruction.Op1Register = (int)(state.rm + state.extraBaseRegisterBase + state.extraBaseRegisterBaseEVEX) + baseReg2;
				if ((state.flags & StateFlags.b) != 0) {
					if (onlySAE)
						instruction.InternalSetSuppressAllExceptions();
					else {
						Debug.Assert((int)RoundingControl.None == 0);
						Debug.Assert((int)RoundingControl.RoundToNearest == 1);
						Debug.Assert((int)RoundingControl.RoundDown == 2);
						Debug.Assert((int)RoundingControl.RoundUp == 3);
						Debug.Assert((int)RoundingControl.RoundTowardZero == 4);
						instruction.InternalRoundingControl = (uint)state.vectorLength + 1;
					}
				}
			}
			else {
				instruction.InternalOp2Kind = OpKind.Memory;
				if ((state.flags & StateFlags.b) != 0)
					instruction.InternalMemorySize = memSizeBroadcast;
				else
					instruction.InternalMemorySize = memSize;
				decoder.ReadOpMem_m64(ref instruction, tupleType);
			}
			instruction.InternalOp3Kind = OpKind.Immediate8;
			instruction.InternalImmediate8 = decoder.ReadIb();
			instruction.InternalOpMask = state.aaa;
			if ((state.flags & StateFlags.z) != 0)
				instruction.InternalSetZeroingMasking();
		}
	}

	sealed class OpCodeHandler_EVEX_VkW : OpCodeHandlerModRM {
		readonly Register baseReg1;
		readonly Register baseReg2;
		readonly Code code;
		readonly TupleType tupleType;
		readonly MemorySize memSize;
		readonly MemorySize memSizeBroadcast;

		public OpCodeHandler_EVEX_VkW(Register baseReg, Code code, TupleType tupleType, MemorySize memSize) {
			baseReg1 = baseReg;
			baseReg2 = baseReg;
			this.code = code;
			this.tupleType = tupleType;
			this.memSize = memSize;
			memSizeBroadcast = memSize;
		}

		public OpCodeHandler_EVEX_VkW(Register baseReg1, Register baseReg2, Code code, TupleType tupleType, MemorySize memSize) {
			this.baseReg1 = baseReg1;
			this.baseReg2 = baseReg2;
			this.code = code;
			this.tupleType = tupleType;
			this.memSize = memSize;
			memSizeBroadcast = memSize;
		}

		public OpCodeHandler_EVEX_VkW(Register baseReg, Code code, TupleType tupleType, MemorySize memSize, MemorySize memSizeBroadcast) {
			baseReg1 = baseReg;
			baseReg2 = baseReg;
			this.code = code;
			this.tupleType = tupleType;
			this.memSize = memSize;
			this.memSizeBroadcast = memSizeBroadcast;
		}

		public OpCodeHandler_EVEX_VkW(Register baseReg1, Register baseReg2, Code code, TupleType tupleType, MemorySize memSize, MemorySize memSizeBroadcast) {
			this.baseReg1 = baseReg1;
			this.baseReg2 = baseReg2;
			this.code = code;
			this.tupleType = tupleType;
			this.memSize = memSize;
			this.memSizeBroadcast = memSizeBroadcast;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.EVEX);
			if ((int)state.vvvv != 0) {
				decoder.SetInvalidInstruction();
				return;
			}
			instruction.InternalCode = code;
			instruction.InternalOpCount = 2;

			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp0Kind = OpKind.Register;
			instruction.Op0Register = (int)(state.reg + state.extraRegisterBase + state.extraRegisterBaseEVEX) + baseReg1;
			if (state.mod == 3) {
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp1Kind = OpKind.Register;
				instruction.Op1Register = (int)(state.rm + state.extraBaseRegisterBase + state.extraBaseRegisterBaseEVEX) + baseReg2;
			}
			else {
				instruction.InternalOp2Kind = OpKind.Memory;
				if ((state.flags & StateFlags.b) != 0)
					instruction.InternalMemorySize = memSizeBroadcast;
				else
					instruction.InternalMemorySize = memSize;
				decoder.ReadOpMem_m64(ref instruction, tupleType);
			}
			instruction.InternalOpMask = state.aaa;
			if ((state.flags & StateFlags.z) != 0)
				instruction.InternalSetZeroingMasking();
		}
	}

	sealed class OpCodeHandler_EVEX_WkV : OpCodeHandlerModRM {
		readonly Register baseReg1;
		readonly Register baseReg2;
		readonly Code code;
		readonly TupleType tupleType;
		readonly MemorySize memSize;

		public OpCodeHandler_EVEX_WkV(Register baseReg, Code code, TupleType tupleType, MemorySize memSize) {
			baseReg1 = baseReg;
			baseReg2 = baseReg;
			this.code = code;
			this.tupleType = tupleType;
			this.memSize = memSize;
		}

		public OpCodeHandler_EVEX_WkV(Register baseReg1, Register baseReg2, Code code, TupleType tupleType, MemorySize memSize) {
			this.baseReg1 = baseReg1;
			this.baseReg2 = baseReg2;
			this.code = code;
			this.tupleType = tupleType;
			this.memSize = memSize;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.EVEX);
			if ((int)state.vvvv != 0) {
				decoder.SetInvalidInstruction();
				return;
			}
			instruction.InternalCode = code;
			instruction.InternalOpCount = 2;

			if (state.mod == 3) {
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp0Kind = OpKind.Register;
				instruction.Op0Register = (int)(state.rm + state.extraBaseRegisterBase + state.extraBaseRegisterBaseEVEX) + baseReg1;
			}
			else {
				instruction.InternalOp1Kind = OpKind.Memory;
				instruction.InternalMemorySize = memSize;
				decoder.ReadOpMem_m64(ref instruction, tupleType);
			}
			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp1Kind = OpKind.Register;
			instruction.Op1Register = (int)(state.reg + state.extraRegisterBase + state.extraRegisterBaseEVEX) + baseReg2;
			instruction.InternalOpMask = state.aaa;
			if ((state.flags & StateFlags.z) != 0)
				instruction.InternalSetZeroingMasking();
		}
	}

	sealed class OpCodeHandler_EVEX_VkM : OpCodeHandlerModRM {
		readonly Register baseReg;
		readonly Code code;
		readonly TupleType tupleType;
		readonly MemorySize memSize;
		readonly MemorySize memSizeBroadcast;

		public OpCodeHandler_EVEX_VkM(Register baseReg, Code code, TupleType tupleType, MemorySize memSize) {
			this.baseReg = baseReg;
			this.code = code;
			this.tupleType = tupleType;
			this.memSize = memSize;
			memSizeBroadcast = memSize;
		}

		public OpCodeHandler_EVEX_VkM(Register baseReg, Code code, TupleType tupleType, MemorySize memSize, MemorySize memSizeBroadcast) {
			this.baseReg = baseReg;
			this.code = code;
			this.tupleType = tupleType;
			this.memSize = memSize;
			this.memSizeBroadcast = memSizeBroadcast;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.EVEX);
			if ((int)state.vvvv != 0) {
				decoder.SetInvalidInstruction();
				return;
			}
			instruction.InternalCode = code;
			instruction.InternalOpCount = 2;

			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp0Kind = OpKind.Register;
			instruction.Op0Register = (int)(state.reg + state.extraRegisterBase + state.extraRegisterBaseEVEX) + baseReg;
			if (state.mod == 3)
				decoder.SetInvalidInstruction();
			else {
				instruction.InternalOp2Kind = OpKind.Memory;
				if ((state.flags & StateFlags.b) != 0)
					instruction.InternalMemorySize = memSizeBroadcast;
				else
					instruction.InternalMemorySize = memSize;
				decoder.ReadOpMem_m64(ref instruction, tupleType);
			}
			instruction.InternalOpMask = state.aaa;
			if ((state.flags & StateFlags.z) != 0)
				instruction.InternalSetZeroingMasking();
		}
	}

	sealed class OpCodeHandler_EVEX_VkWIb : OpCodeHandlerModRM {
		readonly Register baseReg1;
		readonly Register baseReg2;
		readonly Code code;
		readonly TupleType tupleType;
		readonly MemorySize memSize;
		readonly MemorySize memSizeBroadcast;

		public OpCodeHandler_EVEX_VkWIb(Register baseReg, Code code, TupleType tupleType, MemorySize memSize) {
			baseReg1 = baseReg;
			baseReg2 = baseReg;
			this.code = code;
			this.tupleType = tupleType;
			this.memSize = memSize;
			memSizeBroadcast = memSize;
		}

		public OpCodeHandler_EVEX_VkWIb(Register baseReg1, Register baseReg2, Code code, TupleType tupleType, MemorySize memSize) {
			this.baseReg1 = baseReg1;
			this.baseReg2 = baseReg2;
			this.code = code;
			this.tupleType = tupleType;
			this.memSize = memSize;
			memSizeBroadcast = memSize;
		}

		public OpCodeHandler_EVEX_VkWIb(Register baseReg, Code code, TupleType tupleType, MemorySize memSize, MemorySize memSizeBroadcast) {
			baseReg1 = baseReg;
			baseReg2 = baseReg;
			this.code = code;
			this.tupleType = tupleType;
			this.memSize = memSize;
			this.memSizeBroadcast = memSizeBroadcast;
		}

		public OpCodeHandler_EVEX_VkWIb(Register baseReg1, Register baseReg2, Code code, TupleType tupleType, MemorySize memSize, MemorySize memSizeBroadcast) {
			this.baseReg1 = baseReg1;
			this.baseReg2 = baseReg2;
			this.code = code;
			this.tupleType = tupleType;
			this.memSize = memSize;
			this.memSizeBroadcast = memSizeBroadcast;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.EVEX);
			if ((int)state.vvvv != 0) {
				decoder.SetInvalidInstruction();
				return;
			}
			instruction.InternalCode = code;
			instruction.InternalOpCount = 3;

			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp0Kind = OpKind.Register;
			instruction.Op0Register = (int)(state.reg + state.extraRegisterBase + state.extraRegisterBaseEVEX) + baseReg1;
			if (state.mod == 3) {
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp1Kind = OpKind.Register;
				instruction.Op1Register = (int)(state.rm + state.extraBaseRegisterBase + state.extraBaseRegisterBaseEVEX) + baseReg2;
			}
			else {
				instruction.InternalOp2Kind = OpKind.Memory;
				if ((state.flags & StateFlags.b) != 0)
					instruction.InternalMemorySize = memSizeBroadcast;
				else
					instruction.InternalMemorySize = memSize;
				decoder.ReadOpMem_m64(ref instruction, tupleType);
			}
			instruction.InternalOp3Kind = OpKind.Immediate8;
			instruction.InternalImmediate8 = decoder.ReadIb();
			instruction.InternalOpMask = state.aaa;
			if ((state.flags & StateFlags.z) != 0)
				instruction.InternalSetZeroingMasking();
		}
	}

	sealed class OpCodeHandler_EVEX_WkVIb : OpCodeHandlerModRM {
		readonly Register baseReg1;
		readonly Register baseReg2;
		readonly Code code;
		readonly TupleType tupleType;
		readonly MemorySize memSize;
		readonly MemorySize memSizeBroadcast;

		public OpCodeHandler_EVEX_WkVIb(Register baseReg, Code code, TupleType tupleType, MemorySize memSize) {
			baseReg1 = baseReg;
			baseReg2 = baseReg;
			this.code = code;
			this.tupleType = tupleType;
			this.memSize = memSize;
			memSizeBroadcast = memSize;
		}

		public OpCodeHandler_EVEX_WkVIb(Register baseReg1, Register baseReg2, Code code, TupleType tupleType, MemorySize memSize) {
			this.baseReg1 = baseReg1;
			this.baseReg2 = baseReg2;
			this.code = code;
			this.tupleType = tupleType;
			this.memSize = memSize;
			memSizeBroadcast = memSize;
		}

		public OpCodeHandler_EVEX_WkVIb(Register baseReg, Code code, TupleType tupleType, MemorySize memSize, MemorySize memSizeBroadcast) {
			baseReg1 = baseReg;
			baseReg2 = baseReg;
			this.code = code;
			this.tupleType = tupleType;
			this.memSize = memSize;
			this.memSizeBroadcast = memSizeBroadcast;
		}

		public OpCodeHandler_EVEX_WkVIb(Register baseReg1, Register baseReg2, Code code, TupleType tupleType, MemorySize memSize, MemorySize memSizeBroadcast) {
			this.baseReg1 = baseReg1;
			this.baseReg2 = baseReg2;
			this.code = code;
			this.tupleType = tupleType;
			this.memSize = memSize;
			this.memSizeBroadcast = memSizeBroadcast;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.EVEX);
			if ((int)state.vvvv != 0) {
				decoder.SetInvalidInstruction();
				return;
			}
			instruction.InternalCode = code;
			instruction.InternalOpCount = 3;

			if (state.mod == 3) {
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp0Kind = OpKind.Register;
				instruction.Op0Register = (int)(state.rm + state.extraBaseRegisterBase + state.extraBaseRegisterBaseEVEX) + baseReg1;
			}
			else {
				instruction.InternalOp1Kind = OpKind.Memory;
				if ((state.flags & StateFlags.b) != 0)
					instruction.InternalMemorySize = memSizeBroadcast;
				else
					instruction.InternalMemorySize = memSize;
				decoder.ReadOpMem_m64(ref instruction, tupleType);
			}
			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp1Kind = OpKind.Register;
			instruction.Op1Register = (int)(state.reg + state.extraRegisterBase + state.extraRegisterBaseEVEX) + baseReg2;
			instruction.InternalOp3Kind = OpKind.Immediate8;
			instruction.InternalImmediate8 = decoder.ReadIb();
			instruction.InternalOpMask = state.aaa;
			if ((state.flags & StateFlags.z) != 0)
				instruction.InternalSetZeroingMasking();
		}
	}

	sealed class OpCodeHandler_EVEX_HkWIb : OpCodeHandlerModRM {
		readonly Register baseReg1;
		readonly Register baseReg2;
		readonly Code code;
		readonly TupleType tupleType;
		readonly MemorySize memSize;
		readonly MemorySize memSizeBroadcast;

		public OpCodeHandler_EVEX_HkWIb(Register baseReg, Code code, TupleType tupleType, MemorySize memSize) {
			baseReg1 = baseReg;
			baseReg2 = baseReg;
			this.code = code;
			this.tupleType = tupleType;
			this.memSize = memSize;
			memSizeBroadcast = memSize;
		}

		public OpCodeHandler_EVEX_HkWIb(Register baseReg1, Register baseReg2, Code code, TupleType tupleType, MemorySize memSize) {
			this.baseReg1 = baseReg1;
			this.baseReg2 = baseReg2;
			this.code = code;
			this.tupleType = tupleType;
			this.memSize = memSize;
			memSizeBroadcast = memSize;
		}

		public OpCodeHandler_EVEX_HkWIb(Register baseReg, Code code, TupleType tupleType, MemorySize memSize, MemorySize memSizeBroadcast) {
			baseReg1 = baseReg;
			baseReg2 = baseReg;
			this.code = code;
			this.tupleType = tupleType;
			this.memSize = memSize;
			this.memSizeBroadcast = memSizeBroadcast;
		}

		public OpCodeHandler_EVEX_HkWIb(Register baseReg1, Register baseReg2, Code code, TupleType tupleType, MemorySize memSize, MemorySize memSizeBroadcast) {
			this.baseReg1 = baseReg1;
			this.baseReg2 = baseReg2;
			this.code = code;
			this.tupleType = tupleType;
			this.memSize = memSize;
			this.memSizeBroadcast = memSizeBroadcast;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.EVEX);
			instruction.InternalCode = code;
			instruction.InternalOpCount = 3;

			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp0Kind = OpKind.Register;
			instruction.Op0Register = (int)state.vvvv + baseReg1;
			if (state.mod == 3) {
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp1Kind = OpKind.Register;
				instruction.Op1Register = (int)(state.rm + state.extraBaseRegisterBase + state.extraBaseRegisterBaseEVEX) + baseReg2;
			}
			else {
				instruction.InternalOp2Kind = OpKind.Memory;
				if ((state.flags & StateFlags.b) != 0)
					instruction.InternalMemorySize = memSizeBroadcast;
				else
					instruction.InternalMemorySize = memSize;
				decoder.ReadOpMem_m64(ref instruction, tupleType);
			}
			instruction.InternalOp3Kind = OpKind.Immediate8;
			instruction.InternalImmediate8 = decoder.ReadIb();
			instruction.InternalOpMask = state.aaa;
			if ((state.flags & StateFlags.z) != 0)
				instruction.InternalSetZeroingMasking();
		}
	}

	sealed class OpCodeHandler_EVEX_WkVIb_er : OpCodeHandlerModRM {
		readonly Register baseReg1;
		readonly Register baseReg2;
		readonly Code code;
		readonly TupleType tupleType;
		readonly MemorySize memSize;
		readonly MemorySize memSizeBroadcast;
		readonly bool onlySAE;

		public OpCodeHandler_EVEX_WkVIb_er(Register baseReg, Code code, TupleType tupleType, MemorySize memSize, bool onlySAE) {
			baseReg1 = baseReg;
			baseReg2 = baseReg;
			this.code = code;
			this.tupleType = tupleType;
			this.memSize = memSize;
			memSizeBroadcast = memSize;
			this.onlySAE = onlySAE;
		}

		public OpCodeHandler_EVEX_WkVIb_er(Register baseReg1, Register baseReg2, Code code, TupleType tupleType, MemorySize memSize, bool onlySAE) {
			this.baseReg1 = baseReg1;
			this.baseReg2 = baseReg2;
			this.code = code;
			this.tupleType = tupleType;
			this.memSize = memSize;
			memSizeBroadcast = memSize;
			this.onlySAE = onlySAE;
		}

		public OpCodeHandler_EVEX_WkVIb_er(Register baseReg, Code code, TupleType tupleType, MemorySize memSize, MemorySize memSizeBroadcast, bool onlySAE) {
			baseReg1 = baseReg;
			baseReg2 = baseReg;
			this.code = code;
			this.tupleType = tupleType;
			this.memSize = memSize;
			this.memSizeBroadcast = memSizeBroadcast;
			this.onlySAE = onlySAE;
		}

		public OpCodeHandler_EVEX_WkVIb_er(Register baseReg1, Register baseReg2, Code code, TupleType tupleType, MemorySize memSize, MemorySize memSizeBroadcast, bool onlySAE) {
			this.baseReg1 = baseReg1;
			this.baseReg2 = baseReg2;
			this.code = code;
			this.tupleType = tupleType;
			this.memSize = memSize;
			this.memSizeBroadcast = memSizeBroadcast;
			this.onlySAE = onlySAE;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.EVEX);
			if ((int)state.vvvv != 0) {
				decoder.SetInvalidInstruction();
				return;
			}
			instruction.InternalCode = code;
			instruction.InternalOpCount = 3;

			if (state.mod == 3) {
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp0Kind = OpKind.Register;
				instruction.Op0Register = (int)(state.rm + state.extraBaseRegisterBase + state.extraBaseRegisterBaseEVEX) + baseReg1;
				if ((state.flags & StateFlags.b) != 0) {
					if (onlySAE)
						instruction.InternalSetSuppressAllExceptions();
					else {
						Debug.Assert((int)RoundingControl.None == 0);
						Debug.Assert((int)RoundingControl.RoundToNearest == 1);
						Debug.Assert((int)RoundingControl.RoundDown == 2);
						Debug.Assert((int)RoundingControl.RoundUp == 3);
						Debug.Assert((int)RoundingControl.RoundTowardZero == 4);
						instruction.InternalRoundingControl = (uint)state.vectorLength + 1;
					}
				}
			}
			else {
				instruction.InternalOp1Kind = OpKind.Memory;
				if ((state.flags & StateFlags.b) != 0)
					instruction.InternalMemorySize = memSizeBroadcast;
				else
					instruction.InternalMemorySize = memSize;
				decoder.ReadOpMem_m64(ref instruction, tupleType);
			}
			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp1Kind = OpKind.Register;
			instruction.Op1Register = (int)(state.reg + state.extraRegisterBase + state.extraRegisterBaseEVEX) + baseReg2;
			instruction.InternalOp3Kind = OpKind.Immediate8;
			instruction.InternalImmediate8 = decoder.ReadIb();
			instruction.InternalOpMask = state.aaa;
			if ((state.flags & StateFlags.z) != 0)
				instruction.InternalSetZeroingMasking();
		}
	}

	sealed class OpCodeHandler_EVEX_VW_er : OpCodeHandlerModRM {
		readonly Register baseReg1;
		readonly Register baseReg2;
		readonly Code code;
		readonly TupleType tupleType;
		readonly MemorySize memSize;
		readonly MemorySize memSizeBroadcast;
		readonly bool onlySAE;

		public OpCodeHandler_EVEX_VW_er(Register baseReg, Code code, TupleType tupleType, MemorySize memSize, MemorySize memSizeBroadcast, bool onlySAE) {
			baseReg1 = baseReg;
			baseReg2 = baseReg;
			this.code = code;
			this.tupleType = tupleType;
			this.memSize = memSize;
			this.memSizeBroadcast = memSizeBroadcast;
			this.onlySAE = onlySAE;
		}

		public OpCodeHandler_EVEX_VW_er(Register baseReg1, Register baseReg2, Code code, TupleType tupleType, MemorySize memSize, MemorySize memSizeBroadcast, bool onlySAE) {
			this.baseReg1 = baseReg1;
			this.baseReg2 = baseReg2;
			this.code = code;
			this.tupleType = tupleType;
			this.memSize = memSize;
			this.memSizeBroadcast = memSizeBroadcast;
			this.onlySAE = onlySAE;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.EVEX);
			if ((int)state.vvvv != 0) {
				decoder.SetInvalidInstruction();
				return;
			}
			instruction.InternalCode = code;
			instruction.InternalOpCount = 2;

			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp0Kind = OpKind.Register;
			instruction.Op0Register = (int)(state.reg + state.extraRegisterBase + state.extraRegisterBaseEVEX) + baseReg1;
			if (state.mod == 3) {
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp1Kind = OpKind.Register;
				instruction.Op1Register = (int)(state.rm + state.extraBaseRegisterBase + state.extraBaseRegisterBaseEVEX) + baseReg2;
				if ((state.flags & StateFlags.b) != 0) {
					if (onlySAE)
						instruction.InternalSetSuppressAllExceptions();
					else {
						Debug.Assert((int)RoundingControl.None == 0);
						Debug.Assert((int)RoundingControl.RoundToNearest == 1);
						Debug.Assert((int)RoundingControl.RoundDown == 2);
						Debug.Assert((int)RoundingControl.RoundUp == 3);
						Debug.Assert((int)RoundingControl.RoundTowardZero == 4);
						instruction.InternalRoundingControl = (uint)state.vectorLength + 1;
					}
				}
			}
			else {
				instruction.InternalOp2Kind = OpKind.Memory;
				if ((state.flags & StateFlags.b) != 0)
					instruction.InternalMemorySize = memSizeBroadcast;
				else
					instruction.InternalMemorySize = memSize;
				decoder.ReadOpMem_m64(ref instruction, tupleType);
			}
			if (state.aaa != 0 || (state.flags & StateFlags.z) != 0)
				decoder.SetInvalidInstruction();
		}
	}

	sealed class OpCodeHandler_EVEX_VW : OpCodeHandlerModRM {
		readonly Register baseReg1;
		readonly Register baseReg2;
		readonly Code code;
		readonly TupleType tupleType;
		readonly MemorySize memSize;
		readonly MemorySize memSizeBroadcast;

		public OpCodeHandler_EVEX_VW(Register baseReg, Code code, TupleType tupleType, MemorySize memSize) {
			baseReg1 = baseReg;
			baseReg2 = baseReg;
			this.code = code;
			this.tupleType = tupleType;
			this.memSize = memSize;
			memSizeBroadcast = memSize;
		}

		public OpCodeHandler_EVEX_VW(Register baseReg, Code code, TupleType tupleType, MemorySize memSize, MemorySize memSizeBroadcast) {
			baseReg1 = baseReg;
			baseReg2 = baseReg;
			this.code = code;
			this.tupleType = tupleType;
			this.memSize = memSize;
			this.memSizeBroadcast = memSizeBroadcast;
		}

		public OpCodeHandler_EVEX_VW(Register baseReg1, Register baseReg2, Code code, TupleType tupleType, MemorySize memSize, MemorySize memSizeBroadcast) {
			this.baseReg1 = baseReg1;
			this.baseReg2 = baseReg2;
			this.code = code;
			this.tupleType = tupleType;
			this.memSize = memSize;
			this.memSizeBroadcast = memSizeBroadcast;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.EVEX);
			if ((int)state.vvvv != 0) {
				decoder.SetInvalidInstruction();
				return;
			}
			instruction.InternalCode = code;
			instruction.InternalOpCount = 2;

			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp0Kind = OpKind.Register;
			instruction.Op0Register = (int)(state.reg + state.extraRegisterBase + state.extraRegisterBaseEVEX) + baseReg1;
			if (state.mod == 3) {
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp1Kind = OpKind.Register;
				instruction.Op1Register = (int)(state.rm + state.extraBaseRegisterBase + state.extraBaseRegisterBaseEVEX) + baseReg2;
			}
			else {
				instruction.InternalOp2Kind = OpKind.Memory;
				if ((state.flags & StateFlags.b) != 0)
					instruction.InternalMemorySize = memSizeBroadcast;
				else
					instruction.InternalMemorySize = memSize;
				decoder.ReadOpMem_m64(ref instruction, tupleType);
			}
		}
	}

	sealed class OpCodeHandler_EVEX_WV : OpCodeHandlerModRM {
		readonly Register baseReg1;
		readonly Register baseReg2;
		readonly Code code;
		readonly TupleType tupleType;
		readonly MemorySize memSize;
		readonly MemorySize memSizeBroadcast;

		public OpCodeHandler_EVEX_WV(Register baseReg, Code code, TupleType tupleType, MemorySize memSize) {
			baseReg1 = baseReg;
			baseReg2 = baseReg;
			this.code = code;
			this.tupleType = tupleType;
			this.memSize = memSize;
			memSizeBroadcast = memSize;
		}

		public OpCodeHandler_EVEX_WV(Register baseReg, Code code, TupleType tupleType, MemorySize memSize, MemorySize memSizeBroadcast) {
			baseReg1 = baseReg;
			baseReg2 = baseReg;
			this.code = code;
			this.tupleType = tupleType;
			this.memSize = memSize;
			this.memSizeBroadcast = memSizeBroadcast;
		}

		public OpCodeHandler_EVEX_WV(Register baseReg1, Register baseReg2, Code code, TupleType tupleType, MemorySize memSize, MemorySize memSizeBroadcast) {
			this.baseReg1 = baseReg1;
			this.baseReg2 = baseReg2;
			this.code = code;
			this.tupleType = tupleType;
			this.memSize = memSize;
			this.memSizeBroadcast = memSizeBroadcast;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.EVEX);
			if ((int)state.vvvv != 0) {
				decoder.SetInvalidInstruction();
				return;
			}
			instruction.InternalCode = code;
			instruction.InternalOpCount = 2;

			if (state.mod == 3) {
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp0Kind = OpKind.Register;
				instruction.Op0Register = (int)(state.rm + state.extraBaseRegisterBase + state.extraBaseRegisterBaseEVEX) + baseReg2;
			}
			else {
				instruction.InternalOp1Kind = OpKind.Memory;
				if ((state.flags & StateFlags.b) != 0)
					instruction.InternalMemorySize = memSizeBroadcast;
				else
					instruction.InternalMemorySize = memSize;
				decoder.ReadOpMem_m64(ref instruction, tupleType);
			}
			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp1Kind = OpKind.Register;
			instruction.Op1Register = (int)(state.reg + state.extraRegisterBase + state.extraRegisterBaseEVEX) + baseReg1;
		}
	}

	sealed class OpCodeHandler_EVEX_VM : OpCodeHandlerModRM {
		readonly Register baseReg;
		readonly Code code;
		readonly TupleType tupleType;
		readonly MemorySize memSize;
		readonly MemorySize memSizeBroadcast;

		public OpCodeHandler_EVEX_VM(Register baseReg, Code code, TupleType tupleType, MemorySize memSize) {
			this.baseReg = baseReg;
			this.code = code;
			this.tupleType = tupleType;
			this.memSize = memSize;
			memSizeBroadcast = memSize;
		}

		public OpCodeHandler_EVEX_VM(Register baseReg, Code code, TupleType tupleType, MemorySize memSize, MemorySize memSizeBroadcast) {
			this.baseReg = baseReg;
			this.code = code;
			this.tupleType = tupleType;
			this.memSize = memSize;
			this.memSizeBroadcast = memSizeBroadcast;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.EVEX);
			if ((int)state.vvvv != 0) {
				decoder.SetInvalidInstruction();
				return;
			}
			instruction.InternalCode = code;
			instruction.InternalOpCount = 2;

			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp0Kind = OpKind.Register;
			instruction.Op0Register = (int)(state.reg + state.extraRegisterBase + state.extraRegisterBaseEVEX) + baseReg;
			if (state.mod == 3)
				decoder.SetInvalidInstruction();
			else {
				instruction.InternalOp2Kind = OpKind.Memory;
				if ((state.flags & StateFlags.b) != 0)
					instruction.InternalMemorySize = memSizeBroadcast;
				else
					instruction.InternalMemorySize = memSize;
				decoder.ReadOpMem_m64(ref instruction, tupleType);
			}
		}
	}

	sealed class OpCodeHandler_EVEX_VK : OpCodeHandlerModRM {
		readonly Register baseReg;
		readonly Code code;

		public OpCodeHandler_EVEX_VK(Register baseReg, Code code) {
			this.baseReg = baseReg;
			this.code = code;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.EVEX);
			if ((int)state.vvvv != 0) {
				decoder.SetInvalidInstruction();
				return;
			}
			instruction.InternalCode = code;
			instruction.InternalOpCount = 2;

			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp0Kind = OpKind.Register;
			instruction.Op0Register = (int)(state.reg + state.extraRegisterBase + state.extraRegisterBaseEVEX) + baseReg;
			if (state.mod == 3) {
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp1Kind = OpKind.Register;
				instruction.Op1Register = (int)state.rm + Register.K0;
			}
			else
				decoder.SetInvalidInstruction();
		}
	}

	sealed class OpCodeHandler_EVEX_KR : OpCodeHandlerModRM {
		readonly Register baseReg;
		readonly Code code;

		public OpCodeHandler_EVEX_KR(Register baseReg, Code code) {
			this.baseReg = baseReg;
			this.code = code;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.EVEX);
			if ((int)state.vvvv != 0) {
				decoder.SetInvalidInstruction();
				return;
			}
			instruction.InternalCode = code;
			instruction.InternalOpCount = 2;

			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp0Kind = OpKind.Register;
			instruction.Op0Register = (int)state.reg + Register.K0;
			if (state.mod == 3) {
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp1Kind = OpKind.Register;
				instruction.Op1Register = (int)(state.rm + state.extraBaseRegisterBase + state.extraBaseRegisterBaseEVEX) + baseReg;
			}
			else
				decoder.SetInvalidInstruction();
		}
	}

	sealed class OpCodeHandler_EVEX_kkHWIb : OpCodeHandlerModRM {
		readonly Register baseReg;
		readonly Code code;
		readonly TupleType tupleType;
		readonly MemorySize memSize;
		readonly MemorySize memSizeBroadcast;

		public OpCodeHandler_EVEX_kkHWIb(Register baseReg, Code code, TupleType tupleType, MemorySize memSize, MemorySize memSizeBroadcast) {
			this.baseReg = baseReg;
			this.code = code;
			this.tupleType = tupleType;
			this.memSize = memSize;
			this.memSizeBroadcast = memSizeBroadcast;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.EVEX);
			instruction.InternalCode = code;
			instruction.InternalOpCount = 4;

			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp0Kind = OpKind.Register;
			instruction.Op0Register = (int)state.reg + Register.K0;
			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp1Kind = OpKind.Register;
			instruction.Op1Register = (int)state.vvvv + baseReg;
			if (state.mod == 3) {
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp2Kind = OpKind.Register;
				instruction.Op2Register = (int)(state.rm + state.extraBaseRegisterBase + state.extraBaseRegisterBaseEVEX) + baseReg;
				if ((state.flags & StateFlags.b) != 0)
					instruction.InternalSetSuppressAllExceptions();
			}
			else {
				instruction.InternalOp3Kind = OpKind.Memory;
				if ((state.flags & StateFlags.b) != 0)
					instruction.InternalMemorySize = memSizeBroadcast;
				else
					instruction.InternalMemorySize = memSize;
				decoder.ReadOpMem_m64(ref instruction, tupleType);
			}
			instruction.InternalOp4Kind = OpKind.Immediate8;
			instruction.InternalImmediate8 = decoder.ReadByte();
			instruction.InternalOpMask = state.aaa;
		}
	}

	sealed class OpCodeHandler_EVEX_VkHW : OpCodeHandlerModRM {
		readonly Register baseReg1;
		readonly Register baseReg2;
		readonly Register baseReg3;
		readonly Code code;
		readonly TupleType tupleType;
		readonly MemorySize memSize;
		readonly MemorySize memSizeBroadcast;

		public OpCodeHandler_EVEX_VkHW(Register baseReg, Code code, TupleType tupleType, MemorySize memSize) {
			baseReg1 = baseReg;
			baseReg2 = baseReg;
			baseReg3 = baseReg;
			this.code = code;
			this.tupleType = tupleType;
			this.memSize = memSize;
			memSizeBroadcast = memSize;
		}

		public OpCodeHandler_EVEX_VkHW(Register baseReg1, Register baseReg2, Register baseReg3, Code code, TupleType tupleType, MemorySize memSize) {
			this.baseReg1 = baseReg1;
			this.baseReg2 = baseReg2;
			this.baseReg3 = baseReg3;
			this.code = code;
			this.tupleType = tupleType;
			this.memSize = memSize;
			memSizeBroadcast = memSize;
		}

		public OpCodeHandler_EVEX_VkHW(Register baseReg, Code code, TupleType tupleType, MemorySize memSize, MemorySize memSizeBroadcast) {
			baseReg1 = baseReg;
			baseReg2 = baseReg;
			baseReg3 = baseReg;
			this.code = code;
			this.tupleType = tupleType;
			this.memSize = memSize;
			this.memSizeBroadcast = memSizeBroadcast;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.EVEX);
			instruction.InternalCode = code;
			instruction.InternalOpCount = 3;

			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp0Kind = OpKind.Register;
			instruction.Op0Register = (int)(state.reg + state.extraRegisterBase + state.extraRegisterBaseEVEX) + baseReg1;
			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp1Kind = OpKind.Register;
			instruction.Op1Register = (int)state.vvvv + baseReg2;
			if (state.mod == 3) {
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp2Kind = OpKind.Register;
				instruction.Op2Register = (int)(state.rm + state.extraBaseRegisterBase + state.extraBaseRegisterBaseEVEX) + baseReg3;
				if ((state.flags & StateFlags.b) != 0)
					decoder.SetInvalidInstruction();
			}
			else {
				instruction.InternalOp3Kind = OpKind.Memory;
				if ((state.flags & StateFlags.b) != 0)
					instruction.InternalMemorySize = memSizeBroadcast;
				else
					instruction.InternalMemorySize = memSize;
				decoder.ReadOpMem_m64(ref instruction, tupleType);
			}
			instruction.InternalOpMask = state.aaa;
			if ((state.flags & StateFlags.z) != 0)
				instruction.InternalSetZeroingMasking();
		}
	}

	sealed class OpCodeHandler_EVEX_VkHM : OpCodeHandlerModRM {
		readonly Register baseReg1;
		readonly Register baseReg2;
		readonly Code code;
		readonly TupleType tupleType;
		readonly MemorySize memSize;
		readonly MemorySize memSizeBroadcast;

		public OpCodeHandler_EVEX_VkHM(Register baseReg, Code code, TupleType tupleType, MemorySize memSize) {
			baseReg1 = baseReg;
			baseReg2 = baseReg;
			this.code = code;
			this.tupleType = tupleType;
			this.memSize = memSize;
			memSizeBroadcast = memSize;
		}

		public OpCodeHandler_EVEX_VkHM(Register baseReg1, Register baseReg2, Code code, TupleType tupleType, MemorySize memSize) {
			this.baseReg1 = baseReg1;
			this.baseReg2 = baseReg2;
			this.code = code;
			this.tupleType = tupleType;
			this.memSize = memSize;
			memSizeBroadcast = memSize;
		}

		public OpCodeHandler_EVEX_VkHM(Register baseReg, Code code, TupleType tupleType, MemorySize memSize, MemorySize memSizeBroadcast) {
			baseReg1 = baseReg;
			baseReg2 = baseReg;
			this.code = code;
			this.tupleType = tupleType;
			this.memSize = memSize;
			this.memSizeBroadcast = memSizeBroadcast;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.EVEX);
			instruction.InternalCode = code;
			instruction.InternalOpCount = 3;

			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp0Kind = OpKind.Register;
			instruction.Op0Register = (int)(state.reg + state.extraRegisterBase + state.extraRegisterBaseEVEX) + baseReg1;
			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp1Kind = OpKind.Register;
			instruction.Op1Register = (int)state.vvvv + baseReg2;
			if (state.mod == 3)
				decoder.SetInvalidInstruction();
			else {
				instruction.InternalOp3Kind = OpKind.Memory;
				if ((state.flags & StateFlags.b) != 0)
					instruction.InternalMemorySize = memSizeBroadcast;
				else
					instruction.InternalMemorySize = memSize;
				decoder.ReadOpMem_m64(ref instruction, tupleType);
			}
			instruction.InternalOpMask = state.aaa;
			if ((state.flags & StateFlags.z) != 0)
				instruction.InternalSetZeroingMasking();
		}
	}

	sealed class OpCodeHandler_EVEX_VkHWIb : OpCodeHandlerModRM {
		readonly Register baseReg1;
		readonly Register baseReg2;
		readonly Register baseReg3;
		readonly Code code;
		readonly TupleType tupleType;
		readonly MemorySize memSize;
		readonly MemorySize memSizeBroadcast;

		public OpCodeHandler_EVEX_VkHWIb(Register baseReg, Code code, TupleType tupleType, MemorySize memSize) {
			baseReg1 = baseReg;
			baseReg2 = baseReg;
			baseReg3 = baseReg;
			this.code = code;
			this.tupleType = tupleType;
			this.memSize = memSize;
			memSizeBroadcast = memSize;
		}

		public OpCodeHandler_EVEX_VkHWIb(Register baseReg1, Register baseReg2, Register baseReg3, Code code, TupleType tupleType, MemorySize memSize) {
			this.baseReg1 = baseReg1;
			this.baseReg2 = baseReg2;
			this.baseReg3 = baseReg3;
			this.code = code;
			this.tupleType = tupleType;
			this.memSize = memSize;
			memSizeBroadcast = memSize;
		}

		public OpCodeHandler_EVEX_VkHWIb(Register baseReg, Code code, TupleType tupleType, MemorySize memSize, MemorySize memSizeBroadcast) {
			baseReg1 = baseReg;
			baseReg2 = baseReg;
			baseReg3 = baseReg;
			this.code = code;
			this.tupleType = tupleType;
			this.memSize = memSize;
			this.memSizeBroadcast = memSizeBroadcast;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.EVEX);
			instruction.InternalCode = code;
			instruction.InternalOpCount = 4;

			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp0Kind = OpKind.Register;
			instruction.Op0Register = (int)(state.reg + state.extraRegisterBase + state.extraRegisterBaseEVEX) + baseReg1;
			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp1Kind = OpKind.Register;
			instruction.Op1Register = (int)state.vvvv + baseReg2;
			if (state.mod == 3) {
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp2Kind = OpKind.Register;
				instruction.Op2Register = (int)(state.rm + state.extraBaseRegisterBase + state.extraBaseRegisterBaseEVEX) + baseReg3;
				if ((state.flags & StateFlags.b) != 0)
					decoder.SetInvalidInstruction();
			}
			else {
				instruction.InternalOp3Kind = OpKind.Memory;
				if ((state.flags & StateFlags.b) != 0)
					instruction.InternalMemorySize = memSizeBroadcast;
				else
					instruction.InternalMemorySize = memSize;
				decoder.ReadOpMem_m64(ref instruction, tupleType);
			}
			instruction.InternalOp4Kind = OpKind.Immediate8;
			instruction.InternalImmediate8 = decoder.ReadIb();
			instruction.InternalOpMask = state.aaa;
			if ((state.flags & StateFlags.z) != 0)
				instruction.InternalSetZeroingMasking();
		}
	}

	sealed class OpCodeHandler_EVEX_VkHWIb_er : OpCodeHandlerModRM {
		readonly Register baseReg1;
		readonly Register baseReg2;
		readonly Register baseReg3;
		readonly Code code;
		readonly TupleType tupleType;
		readonly MemorySize memSize;
		readonly MemorySize memSizeBroadcast;
		readonly bool onlySAE;

		public OpCodeHandler_EVEX_VkHWIb_er(Register baseReg, Code code, TupleType tupleType, MemorySize memSize, bool onlySAE) {
			baseReg1 = baseReg;
			baseReg2 = baseReg;
			baseReg3 = baseReg;
			this.code = code;
			this.tupleType = tupleType;
			this.memSize = memSize;
			memSizeBroadcast = memSize;
			this.onlySAE = onlySAE;
		}

		public OpCodeHandler_EVEX_VkHWIb_er(Register baseReg1, Register baseReg2, Register baseReg3, Code code, TupleType tupleType, MemorySize memSize, bool onlySAE) {
			this.baseReg1 = baseReg1;
			this.baseReg2 = baseReg2;
			this.baseReg3 = baseReg3;
			this.code = code;
			this.tupleType = tupleType;
			this.memSize = memSize;
			memSizeBroadcast = memSize;
			this.onlySAE = onlySAE;
		}

		public OpCodeHandler_EVEX_VkHWIb_er(Register baseReg, Code code, TupleType tupleType, MemorySize memSize, MemorySize memSizeBroadcast, bool onlySAE) {
			baseReg1 = baseReg;
			baseReg2 = baseReg;
			baseReg3 = baseReg;
			this.code = code;
			this.tupleType = tupleType;
			this.memSize = memSize;
			this.memSizeBroadcast = memSizeBroadcast;
			this.onlySAE = onlySAE;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.EVEX);
			instruction.InternalCode = code;
			instruction.InternalOpCount = 4;

			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp0Kind = OpKind.Register;
			instruction.Op0Register = (int)(state.reg + state.extraRegisterBase + state.extraRegisterBaseEVEX) + baseReg1;
			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp1Kind = OpKind.Register;
			instruction.Op1Register = (int)state.vvvv + baseReg2;
			if (state.mod == 3) {
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp2Kind = OpKind.Register;
				instruction.Op2Register = (int)(state.rm + state.extraBaseRegisterBase + state.extraBaseRegisterBaseEVEX) + baseReg3;
				if ((state.flags & StateFlags.b) != 0) {
					if (onlySAE)
						instruction.InternalSetSuppressAllExceptions();
					else {
						Debug.Assert((int)RoundingControl.None == 0);
						Debug.Assert((int)RoundingControl.RoundToNearest == 1);
						Debug.Assert((int)RoundingControl.RoundDown == 2);
						Debug.Assert((int)RoundingControl.RoundUp == 3);
						Debug.Assert((int)RoundingControl.RoundTowardZero == 4);
						instruction.InternalRoundingControl = (uint)state.vectorLength + 1;
					}
				}
			}
			else {
				instruction.InternalOp3Kind = OpKind.Memory;
				if ((state.flags & StateFlags.b) != 0)
					instruction.InternalMemorySize = memSizeBroadcast;
				else
					instruction.InternalMemorySize = memSize;
				decoder.ReadOpMem_m64(ref instruction, tupleType);
			}
			instruction.InternalOp4Kind = OpKind.Immediate8;
			instruction.InternalImmediate8 = decoder.ReadIb();
			instruction.InternalOpMask = state.aaa;
			if ((state.flags & StateFlags.z) != 0)
				instruction.InternalSetZeroingMasking();
		}
	}

	sealed class OpCodeHandler_EVEX_KkHW : OpCodeHandlerModRM {
		readonly Register baseReg;
		readonly Code code;
		readonly TupleType tupleType;
		readonly MemorySize memSize;
		readonly MemorySize memSizeBroadcast;

		public OpCodeHandler_EVEX_KkHW(Register baseReg, Code code, TupleType tupleType, MemorySize memSize) {
			this.baseReg = baseReg;
			this.code = code;
			this.tupleType = tupleType;
			this.memSize = memSize;
			memSizeBroadcast = memSize;
		}

		public OpCodeHandler_EVEX_KkHW(Register baseReg, Code code, TupleType tupleType, MemorySize memSize, MemorySize memSizeBroadcast) {
			this.baseReg = baseReg;
			this.code = code;
			this.tupleType = tupleType;
			this.memSize = memSize;
			this.memSizeBroadcast = memSizeBroadcast;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.EVEX);
			instruction.InternalCode = code;
			instruction.InternalOpCount = 3;

			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp0Kind = OpKind.Register;
			instruction.Op0Register = (int)state.reg + Register.K0;
			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp1Kind = OpKind.Register;
			instruction.Op1Register = (int)state.vvvv + baseReg;
			if (state.mod == 3) {
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp2Kind = OpKind.Register;
				instruction.Op2Register = (int)(state.rm + state.extraBaseRegisterBase + state.extraBaseRegisterBaseEVEX) + baseReg;
				if ((state.flags & StateFlags.b) != 0)
					decoder.SetInvalidInstruction();
			}
			else {
				instruction.InternalOp3Kind = OpKind.Memory;
				if ((state.flags & StateFlags.b) != 0)
					instruction.InternalMemorySize = memSizeBroadcast;
				else
					instruction.InternalMemorySize = memSize;
				decoder.ReadOpMem_m64(ref instruction, tupleType);
			}
			instruction.InternalOpMask = state.aaa;
			if ((state.flags & StateFlags.z) != 0)
				decoder.SetInvalidInstruction();
		}
	}

	sealed class OpCodeHandler_EVEX_KkHWIb : OpCodeHandlerModRM {
		readonly Register baseReg;
		readonly Code code;
		readonly TupleType tupleType;
		readonly MemorySize memSize;
		readonly MemorySize memSizeBroadcast;

		public OpCodeHandler_EVEX_KkHWIb(Register baseReg, Code code, TupleType tupleType, MemorySize memSize) {
			this.baseReg = baseReg;
			this.code = code;
			this.tupleType = tupleType;
			this.memSize = memSize;
			memSizeBroadcast = memSize;
		}

		public OpCodeHandler_EVEX_KkHWIb(Register baseReg, Code code, TupleType tupleType, MemorySize memSize, MemorySize memSizeBroadcast) {
			this.baseReg = baseReg;
			this.code = code;
			this.tupleType = tupleType;
			this.memSize = memSize;
			this.memSizeBroadcast = memSizeBroadcast;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.EVEX);
			instruction.InternalCode = code;
			instruction.InternalOpCount = 4;

			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp0Kind = OpKind.Register;
			instruction.Op0Register = (int)state.reg + Register.K0;
			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp1Kind = OpKind.Register;
			instruction.Op1Register = (int)state.vvvv + baseReg;
			if (state.mod == 3) {
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp2Kind = OpKind.Register;
				instruction.Op2Register = (int)(state.rm + state.extraBaseRegisterBase + state.extraBaseRegisterBaseEVEX) + baseReg;
				if ((state.flags & StateFlags.b) != 0)
					decoder.SetInvalidInstruction();
			}
			else {
				instruction.InternalOp3Kind = OpKind.Memory;
				if ((state.flags & StateFlags.b) != 0)
					instruction.InternalMemorySize = memSizeBroadcast;
				else
					instruction.InternalMemorySize = memSize;
				decoder.ReadOpMem_m64(ref instruction, tupleType);
			}
			instruction.InternalOp4Kind = OpKind.Immediate8;
			instruction.InternalImmediate8 = decoder.ReadIb();
			instruction.InternalOpMask = state.aaa;
			if ((state.flags & StateFlags.z) != 0)
				decoder.SetInvalidInstruction();
		}
	}

	sealed class OpCodeHandler_EVEX_WkHV : OpCodeHandlerModRM {
		readonly Register baseReg;
		readonly Code code;
		readonly TupleType tupleType;
		readonly MemorySize memSize;
		readonly MemorySize memSizeBroadcast;

		public OpCodeHandler_EVEX_WkHV(Register baseReg, Code code, TupleType tupleType, MemorySize memSize) {
			this.baseReg = baseReg;
			this.code = code;
			this.tupleType = tupleType;
			this.memSize = memSize;
			memSizeBroadcast = memSize;
		}

		public OpCodeHandler_EVEX_WkHV(Register baseReg, Code code, TupleType tupleType, MemorySize memSize, MemorySize memSizeBroadcast) {
			this.baseReg = baseReg;
			this.code = code;
			this.tupleType = tupleType;
			this.memSize = memSize;
			this.memSizeBroadcast = memSizeBroadcast;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.EVEX);
			instruction.InternalCode = code;
			instruction.InternalOpCount = 3;

			if (state.mod == 3) {
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp0Kind = OpKind.Register;
				instruction.Op0Register = (int)(state.rm + state.extraBaseRegisterBase + state.extraBaseRegisterBaseEVEX) + baseReg;
				if ((state.flags & StateFlags.b) != 0)
					decoder.SetInvalidInstruction();
			}
			else {
				instruction.InternalOp1Kind = OpKind.Memory;
				if ((state.flags & StateFlags.b) != 0)
					instruction.InternalMemorySize = memSizeBroadcast;
				else
					instruction.InternalMemorySize = memSize;
				decoder.ReadOpMem_m64(ref instruction, tupleType);
			}
			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp1Kind = OpKind.Register;
			instruction.Op1Register = (int)state.vvvv + baseReg;
			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp2Kind = OpKind.Register;
			instruction.Op2Register = (int)(state.reg + state.extraRegisterBase + state.extraRegisterBaseEVEX) + baseReg;
			instruction.InternalOpMask = state.aaa;
			if ((state.flags & StateFlags.z) != 0)
				instruction.InternalSetZeroingMasking();
		}
	}

	sealed class OpCodeHandler_EVEX_VHWIb : OpCodeHandlerModRM {
		readonly Register baseReg;
		readonly Code code;
		readonly TupleType tupleType;
		readonly MemorySize memSize;

		public OpCodeHandler_EVEX_VHWIb(Register baseReg, Code code, TupleType tupleType, MemorySize memSize) {
			this.baseReg = baseReg;
			this.code = code;
			this.tupleType = tupleType;
			this.memSize = memSize;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.EVEX);
			instruction.InternalCode = code;
			instruction.InternalOpCount = 4;

			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp0Kind = OpKind.Register;
			instruction.Op0Register = (int)(state.reg + state.extraRegisterBase + state.extraRegisterBaseEVEX) + baseReg;
			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp1Kind = OpKind.Register;
			instruction.Op1Register = (int)state.vvvv + baseReg;
			if (state.mod == 3) {
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp2Kind = OpKind.Register;
				instruction.Op2Register = (int)(state.rm + state.extraBaseRegisterBase + state.extraBaseRegisterBaseEVEX) + baseReg;
			}
			else {
				instruction.InternalOp3Kind = OpKind.Memory;
				instruction.InternalMemorySize = memSize;
				decoder.ReadOpMem_m64(ref instruction, tupleType);
			}
			instruction.InternalOp4Kind = OpKind.Immediate8;
			instruction.InternalImmediate8 = decoder.ReadIb();
		}
	}

	sealed class OpCodeHandler_EVEX_VHW : OpCodeHandlerModRM {
		readonly Register baseReg1;
		readonly Register baseReg2;
		readonly Register baseReg3;
		readonly Code codeR;
		readonly Code codeM;
		readonly TupleType tupleType;
		readonly MemorySize memSize;

		public OpCodeHandler_EVEX_VHW(Register baseReg, Code codeR, Code codeM, TupleType tupleType, MemorySize memSize) {
			baseReg1 = baseReg;
			baseReg2 = baseReg;
			baseReg3 = baseReg;
			this.codeR = codeR;
			this.codeM = codeM;
			this.tupleType = tupleType;
			this.memSize = memSize;
		}

		public OpCodeHandler_EVEX_VHW(Register baseReg, Code code, TupleType tupleType, MemorySize memSize) {
			baseReg1 = baseReg;
			baseReg2 = baseReg;
			baseReg3 = baseReg;
			codeR = code;
			codeM = code;
			this.tupleType = tupleType;
			this.memSize = memSize;
		}

		public OpCodeHandler_EVEX_VHW(Register baseReg1, Register baseReg2, Register baseReg3, Code code, TupleType tupleType, MemorySize memSize) {
			this.baseReg1 = baseReg1;
			this.baseReg2 = baseReg2;
			this.baseReg3 = baseReg3;
			codeR = code;
			codeM = code;
			this.tupleType = tupleType;
			this.memSize = memSize;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.EVEX);
			instruction.InternalOpCount = 3;

			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp0Kind = OpKind.Register;
			instruction.Op0Register = (int)(state.reg + state.extraRegisterBase + state.extraRegisterBaseEVEX) + baseReg1;
			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp1Kind = OpKind.Register;
			instruction.Op1Register = (int)state.vvvv + baseReg2;
			if (state.mod == 3) {
				instruction.InternalCode = codeR;
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp2Kind = OpKind.Register;
				instruction.Op2Register = (int)(state.rm + state.extraBaseRegisterBase + state.extraBaseRegisterBaseEVEX) + baseReg3;
			}
			else {
				instruction.InternalCode = codeM;
				instruction.InternalOp3Kind = OpKind.Memory;
				instruction.InternalMemorySize = memSize;
				decoder.ReadOpMem_m64(ref instruction, tupleType);
			}
		}
	}

	sealed class OpCodeHandler_EVEX_VHM : OpCodeHandlerModRM {
		readonly Register baseReg;
		readonly Code code;
		readonly TupleType tupleType;
		readonly MemorySize memSize;

		public OpCodeHandler_EVEX_VHM(Register baseReg, Code code, TupleType tupleType, MemorySize memSize) {
			this.baseReg = baseReg;
			this.code = code;
			this.tupleType = tupleType;
			this.memSize = memSize;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.EVEX);
			instruction.InternalCode = code;
			instruction.InternalOpCount = 3;

			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp0Kind = OpKind.Register;
			instruction.Op0Register = (int)(state.reg + state.extraRegisterBase + state.extraRegisterBaseEVEX) + baseReg;
			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp1Kind = OpKind.Register;
			instruction.Op1Register = (int)state.vvvv + baseReg;
			if (state.mod == 3)
				decoder.SetInvalidInstruction();
			else {
				instruction.InternalOp3Kind = OpKind.Memory;
				instruction.InternalMemorySize = memSize;
				decoder.ReadOpMem_m64(ref instruction, tupleType);
			}
		}
	}

	sealed class OpCodeHandler_EVEX_Gv_W_er : OpCodeHandlerModRM {
		readonly Register baseReg;
		readonly Code codeW0;
		readonly Code codeW1;
		readonly TupleType tupleType;
		readonly MemorySize memSize;
		readonly bool onlySAE;

		public OpCodeHandler_EVEX_Gv_W_er(Register baseReg, Code codeW0, Code codeW1, TupleType tupleType, MemorySize memSize, bool onlySAE) {
			this.baseReg = baseReg;
			this.codeW0 = codeW0;
			this.codeW1 = codeW1;
			this.tupleType = tupleType;
			this.memSize = memSize;
			this.onlySAE = onlySAE;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.EVEX);
			if ((int)state.vvvv != 0) {
				decoder.SetInvalidInstruction();
				return;
			}
			instruction.InternalOpCount = 2;
			if ((state.flags & StateFlags.W) != 0) {
				instruction.InternalCode = codeW1;
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp0Kind = OpKind.Register;
				instruction.Op0Register = (int)(state.reg + state.extraRegisterBase) + Register.RAX;
			}
			else {
				instruction.InternalCode = codeW0;
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp0Kind = OpKind.Register;
				instruction.Op0Register = (int)(state.reg + state.extraRegisterBase) + Register.EAX;
			}
			if (state.mod == 3) {
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp1Kind = OpKind.Register;
				instruction.Op1Register = (int)(state.rm + state.extraBaseRegisterBase + state.extraBaseRegisterBaseEVEX) + baseReg;
				if ((state.flags & StateFlags.b) != 0) {
					if (onlySAE)
						instruction.InternalSetSuppressAllExceptions();
					else {
						Debug.Assert((int)RoundingControl.None == 0);
						Debug.Assert((int)RoundingControl.RoundToNearest == 1);
						Debug.Assert((int)RoundingControl.RoundDown == 2);
						Debug.Assert((int)RoundingControl.RoundUp == 3);
						Debug.Assert((int)RoundingControl.RoundTowardZero == 4);
						instruction.InternalRoundingControl = (uint)state.vectorLength + 1;
					}
				}
			}
			else {
				instruction.InternalOp2Kind = OpKind.Memory;
				instruction.InternalMemorySize = memSize;
				decoder.ReadOpMem_m64(ref instruction, tupleType);
			}
		}
	}

	sealed class OpCodeHandler_EVEX_VX_Ev : OpCodeHandlerModRM {
		readonly Code code32;
		readonly Code code64;
		readonly TupleType tupleType;

		public OpCodeHandler_EVEX_VX_Ev(Code code32, Code code64, TupleType tupleType) {
			this.code32 = code32;
			this.code64 = code64;
			this.tupleType = tupleType;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.EVEX);
			if ((int)state.vvvv != 0) {
				decoder.SetInvalidInstruction();
				return;
			}
			Register gpr;
			if ((state.flags & StateFlags.W) != 0) {
				instruction.InternalCode = code64;
				gpr = Register.RAX;
			}
			else {
				instruction.InternalCode = code32;
				gpr = Register.EAX;
			}
			instruction.InternalOpCount = 2;
			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp0Kind = OpKind.Register;
			instruction.Op0Register = (int)(state.reg + state.extraRegisterBase + state.extraRegisterBaseEVEX) + Register.XMM0;
			if (state.mod == 3) {
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp1Kind = OpKind.Register;
				instruction.Op1Register = (int)(state.rm + state.extraBaseRegisterBase + state.extraBaseRegisterBaseEVEX) + gpr;
			}
			else {
				instruction.InternalOp2Kind = OpKind.Memory;
				if ((state.flags & StateFlags.W) != 0)
					instruction.InternalMemorySize = MemorySize.UInt64;
				else
					instruction.InternalMemorySize = MemorySize.UInt32;
				decoder.ReadOpMem_m64(ref instruction, tupleType);
			}
		}
	}

	sealed class OpCodeHandler_EVEX_Ev_VX : OpCodeHandlerModRM {
		readonly Code code32;
		readonly Code code64;
		readonly TupleType tupleType;

		public OpCodeHandler_EVEX_Ev_VX(Code code32, Code code64, TupleType tupleType) {
			this.code32 = code32;
			this.code64 = code64;
			this.tupleType = tupleType;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.EVEX);
			if ((int)state.vvvv != 0) {
				decoder.SetInvalidInstruction();
				return;
			}
			Register gpr;
			if ((state.flags & StateFlags.W) != 0) {
				instruction.InternalCode = code64;
				gpr = Register.RAX;
			}
			else {
				instruction.InternalCode = code32;
				gpr = Register.EAX;
			}
			instruction.InternalOpCount = 2;
			if (state.mod == 3) {
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp0Kind = OpKind.Register;
				instruction.Op0Register = (int)(state.rm + state.extraBaseRegisterBase + state.extraBaseRegisterBaseEVEX) + gpr;
			}
			else {
				instruction.InternalOp1Kind = OpKind.Memory;
				if ((state.flags & StateFlags.W) != 0)
					instruction.InternalMemorySize = MemorySize.UInt64;
				else
					instruction.InternalMemorySize = MemorySize.UInt32;
				decoder.ReadOpMem_m64(ref instruction, tupleType);
			}
			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp1Kind = OpKind.Register;
			instruction.Op1Register = (int)(state.reg + state.extraRegisterBase + state.extraRegisterBaseEVEX) + Register.XMM0;
		}
	}

	sealed class OpCodeHandler_EVEX_Ev_VX_Ib : OpCodeHandlerModRM {
		readonly Register baseReg;
		readonly Code code32;
		readonly Code code64;
		readonly TupleType tupleType;
		readonly MemorySize memSize32;
		readonly MemorySize memSize64;

		public OpCodeHandler_EVEX_Ev_VX_Ib(Register baseReg, Code code32, Code code64, TupleType tupleType, MemorySize memSize32, MemorySize memSize64) {
			this.baseReg = baseReg;
			this.code32 = code32;
			this.code64 = code64;
			this.tupleType = tupleType;
			this.memSize32 = memSize32;
			this.memSize64 = memSize64;
		}

		public OpCodeHandler_EVEX_Ev_VX_Ib(Register baseReg, Code code32, Code code64) {
			this.baseReg = baseReg;
			this.code32 = code32;
			this.code64 = code64;
			tupleType = 0;
			memSize32 = MemorySize.Unknown;
			memSize64 = MemorySize.Unknown;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.EVEX);
			if ((int)state.vvvv != 0) {
				decoder.SetInvalidInstruction();
				return;
			}
			Register gpr;
			if ((state.flags & StateFlags.W) != 0) {
				instruction.InternalCode = code64;
				gpr = Register.RAX;
			}
			else {
				instruction.InternalCode = code32;
				gpr = Register.EAX;
			}
			instruction.InternalOpCount = 3;
			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp0Kind = OpKind.Register;
			instruction.Op0Register = (int)(state.reg + state.extraRegisterBase + state.extraRegisterBaseEVEX) + gpr;
			if (state.mod == 3) {
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp1Kind = OpKind.Register;
				instruction.Op1Register = (int)(state.rm + state.extraBaseRegisterBase + state.extraBaseRegisterBaseEVEX) + baseReg;
			}
			else {
				instruction.InternalOp2Kind = OpKind.Memory;
				if ((state.flags & StateFlags.W) != 0)
					instruction.InternalMemorySize = memSize64;
				else
					instruction.InternalMemorySize = memSize32;
				decoder.ReadOpMem_m64(ref instruction, tupleType);
				if (memSize32 == MemorySize.Unknown)
					decoder.SetInvalidInstruction();
			}
			instruction.InternalOp3Kind = OpKind.Immediate8;
			instruction.InternalImmediate8 = decoder.ReadByte();
		}
	}

	sealed class OpCodeHandler_EVEX_MV : OpCodeHandlerModRM {
		readonly Register baseReg;
		readonly Code code;
		readonly TupleType tupleType;
		readonly MemorySize memSize;

		public OpCodeHandler_EVEX_MV(Register baseReg, Code code, TupleType tupleType, MemorySize memSize) {
			this.baseReg = baseReg;
			this.code = code;
			this.tupleType = tupleType;
			this.memSize = memSize;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.EVEX);
			if ((int)state.vvvv != 0) {
				decoder.SetInvalidInstruction();
				return;
			}
			instruction.InternalCode = code;
			instruction.InternalOpCount = 2;

			if (state.mod == 3)
				decoder.SetInvalidInstruction();
			else {
				instruction.InternalOp1Kind = OpKind.Memory;
				instruction.InternalMemorySize = memSize;
				decoder.ReadOpMem_m64(ref instruction, tupleType);
			}
			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp1Kind = OpKind.Register;
			instruction.Op1Register = (int)(state.reg + state.extraRegisterBase + state.extraRegisterBaseEVEX) + baseReg;
		}
	}

	sealed class OpCodeHandler_EVEX_VkEv_REXW : OpCodeHandlerModRM {
		readonly Register baseReg;
		readonly Code code32;
		readonly Code code64;

		public OpCodeHandler_EVEX_VkEv_REXW(Register baseReg, Code code32) {
			this.baseReg = baseReg;
			this.code32 = code32;
			code64 = Code.INVALID;
		}

		public OpCodeHandler_EVEX_VkEv_REXW(Register baseReg, Code code32, Code code64) {
			this.baseReg = baseReg;
			this.code32 = code32;
			this.code64 = code64;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.EVEX);
			if ((int)state.vvvv != 0) {
				decoder.SetInvalidInstruction();
				return;
			}
			if ((state.flags & StateFlags.W) != 0) {
				instruction.InternalCode = code64;
				if (code64 == Code.INVALID)
					decoder.SetInvalidInstruction();
			}
			else
				instruction.InternalCode = code32;
			instruction.InternalOpCount = 2;

			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp0Kind = OpKind.Register;
			instruction.Op0Register = (int)(state.reg + state.extraRegisterBase + state.extraRegisterBaseEVEX) + baseReg;
			if (state.mod == 3) {
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp1Kind = OpKind.Register;
				instruction.Op1Register = (int)(state.rm + state.extraBaseRegisterBase + state.extraBaseRegisterBaseEVEX) + ((state.flags & StateFlags.W) != 0 ? Register.RAX : Register.EAX);
			}
			else
				decoder.SetInvalidInstruction();
			instruction.InternalOpMask = state.aaa;
			if ((state.flags & StateFlags.z) != 0)
				instruction.InternalSetZeroingMasking();
		}
	}

	sealed class OpCodeHandler_EVEX_Vk_VSIB : OpCodeHandlerModRM {
		readonly Register baseReg;
		readonly Register vsibBase;
		readonly Code code;
		readonly TupleType tupleType;
		readonly MemorySize memSize;
		readonly MemorySize memSizeBroadcast;

		public OpCodeHandler_EVEX_Vk_VSIB(Register baseReg, Register vsibBase, Code code, TupleType tupleType, MemorySize memSize) {
			this.baseReg = baseReg;
			this.vsibBase = vsibBase;
			this.code = code;
			this.tupleType = tupleType;
			this.memSize = memSize;
			memSizeBroadcast = memSize;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.EVEX);
			if (((int)state.vvvv & 0xF) != 0) {
				decoder.SetInvalidInstruction();
				return;
			}
			instruction.InternalCode = code;
			instruction.InternalOpCount = 2;

			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp0Kind = OpKind.Register;
			instruction.Op0Register = (int)(state.reg + state.extraRegisterBase + state.extraRegisterBaseEVEX) + baseReg;
			if (state.mod == 3)
				decoder.SetInvalidInstruction();
			else {
				instruction.InternalOp2Kind = OpKind.Memory;
				if ((state.flags & StateFlags.b) != 0)
					instruction.InternalMemorySize = memSizeBroadcast;
				else
					instruction.InternalMemorySize = memSize;
				decoder.ReadOpMem_VSIB_m64(ref instruction, vsibBase, tupleType);
			}
			instruction.InternalOpMask = state.aaa;
			if ((state.flags & StateFlags.z) != 0)
				instruction.InternalSetZeroingMasking();
		}
	}

	sealed class OpCodeHandler_EVEX_VSIB_k1_VX : OpCodeHandlerModRM {
		readonly Register vsibIndex;
		readonly Register baseReg;
		readonly Code code;
		readonly TupleType tupleType;
		readonly MemorySize memSize;

		public OpCodeHandler_EVEX_VSIB_k1_VX(Register vsibIndex, Register baseReg, Code code, TupleType tupleType, MemorySize memSize) {
			this.vsibIndex = vsibIndex;
			this.baseReg = baseReg;
			this.code = code;
			this.tupleType = tupleType;
			this.memSize = memSize;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.EVEX);
			if (((int)state.vvvv & 0xF) != 0) {
				decoder.SetInvalidInstruction();
				return;
			}
			instruction.InternalCode = code;
			instruction.InternalOpCount = 2;

			if (state.mod == 3)
				decoder.SetInvalidInstruction();
			else {
				instruction.InternalOp1Kind = OpKind.Memory;
				instruction.InternalMemorySize = memSize;
				decoder.ReadOpMem_VSIB_m64(ref instruction, vsibIndex, tupleType);
			}
			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp1Kind = OpKind.Register;
			instruction.Op1Register = (int)(state.reg + state.extraRegisterBase + state.extraRegisterBaseEVEX) + baseReg;
			instruction.InternalOpMask = state.aaa;
		}
	}

	sealed class OpCodeHandler_EVEX_VSIB_k1 : OpCodeHandlerModRM {
		readonly Register vsibIndex;
		readonly Code code;
		readonly TupleType tupleType;
		readonly MemorySize memSize;

		public OpCodeHandler_EVEX_VSIB_k1(Register vsibIndex, Code code, TupleType tupleType, MemorySize memSize) {
			this.vsibIndex = vsibIndex;
			this.code = code;
			this.tupleType = tupleType;
			this.memSize = memSize;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.EVEX);
			if (((int)state.vvvv & 0xF) != 0) {
				decoder.SetInvalidInstruction();
				return;
			}
			instruction.InternalCode = code;
			instruction.InternalOpCount = 1;

			if (state.mod == 3)
				decoder.SetInvalidInstruction();
			else {
				instruction.InternalOp1Kind = OpKind.Memory;
				instruction.InternalMemorySize = memSize;
				decoder.ReadOpMem_VSIB_m64(ref instruction, vsibIndex, tupleType);
			}
			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp1Kind = OpKind.Register;
			instruction.Op1Register = (int)state.reg + Register.K0;
			instruction.InternalOpMask = state.aaa;
		}
	}

	sealed class OpCodeHandler_EVEX_GvM_VX_Ib : OpCodeHandlerModRM {
		readonly Register baseReg;
		readonly Code code32;
		readonly Code code64;
		readonly TupleType tupleType32;
		readonly TupleType tupleType64;
		readonly MemorySize memSize32;
		readonly MemorySize memSize64;

		public OpCodeHandler_EVEX_GvM_VX_Ib(Register baseReg, Code code32, Code code64, TupleType tupleType32, TupleType tupleType64, MemorySize memSize32, MemorySize memSize64) {
			this.baseReg = baseReg;
			this.code32 = code32;
			this.code64 = code64;
			this.tupleType32 = tupleType32;
			this.tupleType64 = tupleType64;
			this.memSize32 = memSize32;
			this.memSize64 = memSize64;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.EVEX);
			if ((int)state.vvvv != 0) {
				decoder.SetInvalidInstruction();
				return;
			}
			if ((state.flags & StateFlags.W) != 0)
				instruction.InternalCode = code64;
			else
				instruction.InternalCode = code32;
			instruction.InternalOpCount = 3;
			if (state.mod == 3) {
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp0Kind = OpKind.Register;
				instruction.Op0Register = (int)(state.rm + state.extraBaseRegisterBase + state.extraBaseRegisterBaseEVEX) + ((state.flags & StateFlags.W) != 0 ? Register.RAX : Register.EAX);
			}
			else {
				instruction.InternalOp1Kind = OpKind.Memory;
				if ((state.flags & StateFlags.W) != 0) {
					instruction.InternalMemorySize = memSize64;
					decoder.ReadOpMem_m64(ref instruction, tupleType64);
				}
				else {
					instruction.InternalMemorySize = memSize32;
					decoder.ReadOpMem_m64(ref instruction, tupleType32);
				}
			}
			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp1Kind = OpKind.Register;
			instruction.Op1Register = (int)(state.reg + state.extraRegisterBase + state.extraRegisterBaseEVEX) + baseReg;
			instruction.InternalOp3Kind = OpKind.Immediate8;
			instruction.InternalImmediate8 = decoder.ReadByte();
		}
	}

	sealed class OpCodeHandler_EVEX_KkWIb : OpCodeHandlerModRM {
		readonly Register baseReg;
		readonly Code code;
		readonly TupleType tupleType;
		readonly MemorySize memSize;
		readonly MemorySize memSizeBroadcast;

		public OpCodeHandler_EVEX_KkWIb(Register baseReg, Code code, TupleType tupleType, MemorySize memSize) {
			this.baseReg = baseReg;
			this.code = code;
			this.tupleType = tupleType;
			this.memSize = memSize;
			memSizeBroadcast = memSize;
		}

		public OpCodeHandler_EVEX_KkWIb(Register baseReg, Code code, TupleType tupleType, MemorySize memSize, MemorySize memSizeBroadcast) {
			this.baseReg = baseReg;
			this.code = code;
			this.tupleType = tupleType;
			this.memSize = memSize;
			this.memSizeBroadcast = memSizeBroadcast;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.EVEX);
			if ((int)state.vvvv != 0) {
				decoder.SetInvalidInstruction();
				return;
			}
			instruction.InternalCode = code;
			instruction.InternalOpCount = 3;

			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp0Kind = OpKind.Register;
			instruction.Op0Register = (int)state.reg + Register.K0;
			if (state.mod == 3) {
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp1Kind = OpKind.Register;
				instruction.Op1Register = (int)(state.rm + state.extraBaseRegisterBase + state.extraBaseRegisterBaseEVEX) + baseReg;
			}
			else {
				instruction.InternalOp2Kind = OpKind.Memory;
				if ((state.flags & StateFlags.b) != 0)
					instruction.InternalMemorySize = memSizeBroadcast;
				else
					instruction.InternalMemorySize = memSize;
				decoder.ReadOpMem_m64(ref instruction, tupleType);
			}
			instruction.InternalOp3Kind = OpKind.Immediate8;
			instruction.InternalImmediate8 = decoder.ReadIb();
			instruction.InternalOpMask = state.aaa;
			if ((state.flags & StateFlags.z) != 0)
				instruction.InternalSetZeroingMasking();
		}
	}
}
#endif

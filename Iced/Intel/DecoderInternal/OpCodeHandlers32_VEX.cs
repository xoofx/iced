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

#if !NO_DECODER32 && !NO_DECODER
using System;
using System.Diagnostics;

namespace Iced.Intel.DecoderInternal.OpCodeHandlers32 {
	sealed class OpCodeHandler_VEX2 : OpCodeHandlerModRM {
		readonly OpCodeHandler handlerMem;

		public OpCodeHandler_VEX2(OpCodeHandler handlerMem) => this.handlerMem = handlerMem ?? throw new ArgumentNullException(nameof(handlerMem));

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			if (decoder.state.mod != 3)
				handlerMem.Decode(decoder, ref instruction);
			else
				decoder.VEX2(ref instruction);
		}
	}

	sealed class OpCodeHandler_VEX3 : OpCodeHandlerModRM {
		readonly OpCodeHandler handlerMem;

		public OpCodeHandler_VEX3(OpCodeHandler handlerMem) => this.handlerMem = handlerMem ?? throw new ArgumentNullException(nameof(handlerMem));

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			if (decoder.state.mod != 3)
				handlerMem.Decode(decoder, ref instruction);
			else
				decoder.VEX3(ref instruction);
		}
	}

	sealed class OpCodeHandler_VEX_VHEv : OpCodeHandlerModRM {
		readonly Register baseReg;
		readonly Code codeW0;

		public OpCodeHandler_VEX_VHEv(Register baseReg, Code codeW0, Code codeW1) {
			this.baseReg = baseReg;
			this.codeW0 = codeW0;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.VEX);
			instruction.InternalCode = codeW0;
			instruction.InternalOpCount = 3;
			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp0Kind = OpKind.Register;
			instruction.Op0Register = (int)state.reg + baseReg;
			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp1Kind = OpKind.Register;
			instruction.Op1Register = (int)state.vvvv + baseReg;
			if (state.mod == 3) {
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp2Kind = OpKind.Register;
				instruction.Op2Register = (int)state.rm + Register.EAX;
			}
			else {
				instruction.InternalOp3Kind = OpKind.Memory;
				instruction.InternalMemorySize = MemorySize.Int32;
				decoder.ReadOpMem_m32(ref instruction);
			}
		}
	}

	sealed class OpCodeHandler_VEX_VHEvIb : OpCodeHandlerModRM {
		readonly Register baseReg;
		readonly Code codeW0;
		readonly MemorySize memSizeW0;

		public OpCodeHandler_VEX_VHEvIb(Register baseReg, Code codeW0, Code codeW1, MemorySize memSizeW0, MemorySize memSizeW1) {
			this.baseReg = baseReg;
			this.codeW0 = codeW0;
			this.memSizeW0 = memSizeW0;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.VEX);
			instruction.InternalCode = codeW0;
			instruction.InternalOpCount = 4;
			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp0Kind = OpKind.Register;
			instruction.Op0Register = (int)state.reg + baseReg;
			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp1Kind = OpKind.Register;
			instruction.Op1Register = (int)state.vvvv + baseReg;
			if (state.mod == 3) {
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp2Kind = OpKind.Register;
				instruction.Op2Register = (int)state.rm + Register.EAX;
			}
			else {
				instruction.InternalOp3Kind = OpKind.Memory;
				instruction.InternalMemorySize = memSizeW0;
				decoder.ReadOpMem_m32(ref instruction);
			}
			instruction.InternalOp4Kind = OpKind.Immediate8;
			instruction.InternalImmediate8 = decoder.ReadByte();
		}
	}

	sealed class OpCodeHandler_VEX_VW : OpCodeHandlerModRM {
		readonly Register baseReg1;
		readonly Register baseReg2;
		readonly Code code;
		readonly MemorySize memSize;

		public OpCodeHandler_VEX_VW(Register baseReg, Code code, MemorySize memSize) {
			baseReg1 = baseReg;
			baseReg2 = baseReg;
			this.code = code;
			this.memSize = memSize;
		}

		public OpCodeHandler_VEX_VW(Register baseReg1, Register baseReg2, Code code, MemorySize memSize) {
			this.baseReg1 = baseReg1;
			this.baseReg2 = baseReg2;
			this.code = code;
			this.memSize = memSize;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.VEX);
			if ((int)state.vvvv != 0) {
				decoder.SetInvalidInstruction();
				return;
			}
			instruction.InternalCode = code;
			instruction.InternalOpCount = 2;
			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp0Kind = OpKind.Register;
			instruction.Op0Register = (int)state.reg + baseReg1;
			if (state.mod == 3) {
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp1Kind = OpKind.Register;
				instruction.Op1Register = (int)state.rm + baseReg2;
			}
			else {
				instruction.InternalOp2Kind = OpKind.Memory;
				instruction.InternalMemorySize = memSize;
				decoder.ReadOpMem_m32(ref instruction);
			}
		}
	}

	sealed class OpCodeHandler_VEX_VX_Ev : OpCodeHandlerModRM {
		readonly Code code32;

		public OpCodeHandler_VEX_VX_Ev(Code code32, Code code64) => this.code32 = code32;

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.VEX);
			if ((int)state.vvvv != 0) {
				decoder.SetInvalidInstruction();
				return;
			}
			instruction.InternalCode = code32;
			instruction.InternalOpCount = 2;
			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp0Kind = OpKind.Register;
			instruction.Op0Register = (int)state.reg + Register.XMM0;
			if (state.mod == 3) {
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp1Kind = OpKind.Register;
				instruction.Op1Register = (int)state.rm + Register.EAX;
			}
			else {
				instruction.InternalOp2Kind = OpKind.Memory;
				instruction.InternalMemorySize = MemorySize.UInt32;
				decoder.ReadOpMem_m32(ref instruction);
			}
		}
	}

	sealed class OpCodeHandler_VEX_Ev_VX : OpCodeHandlerModRM {
		readonly Code code32;

		public OpCodeHandler_VEX_Ev_VX(Code code32, Code code64) => this.code32 = code32;

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.VEX);
			if ((int)state.vvvv != 0) {
				decoder.SetInvalidInstruction();
				return;
			}
			instruction.InternalCode = code32;
			instruction.InternalOpCount = 2;
			if (state.mod == 3) {
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp0Kind = OpKind.Register;
				instruction.Op0Register = (int)state.rm + Register.EAX;
			}
			else {
				instruction.InternalOp1Kind = OpKind.Memory;
				instruction.InternalMemorySize = MemorySize.UInt32;
				decoder.ReadOpMem_m32(ref instruction);
			}
			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp1Kind = OpKind.Register;
			instruction.Op1Register = (int)state.reg + Register.XMM0;
		}
	}

	sealed class OpCodeHandler_VEX_WV : OpCodeHandlerModRM {
		readonly Register baseReg1;
		readonly Register baseReg2;
		readonly Code code;
		readonly MemorySize memSize;

		public OpCodeHandler_VEX_WV(Register baseReg, Code code, MemorySize memSize) {
			baseReg1 = baseReg;
			baseReg2 = baseReg;
			this.code = code;
			this.memSize = memSize;
		}

		public OpCodeHandler_VEX_WV(Register baseReg1, Register baseReg2, Code code, MemorySize memSize) {
			this.baseReg1 = baseReg1;
			this.baseReg2 = baseReg2;
			this.code = code;
			this.memSize = memSize;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.VEX);
			if ((int)state.vvvv != 0) {
				decoder.SetInvalidInstruction();
				return;
			}
			instruction.InternalCode = code;
			instruction.InternalOpCount = 2;
			if (state.mod == 3) {
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp0Kind = OpKind.Register;
				instruction.Op0Register = (int)state.rm + baseReg2;
			}
			else {
				instruction.InternalOp1Kind = OpKind.Memory;
				instruction.InternalMemorySize = memSize;
				decoder.ReadOpMem_m32(ref instruction);
			}
			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp1Kind = OpKind.Register;
			instruction.Op1Register = (int)state.reg + baseReg1;
		}
	}

	sealed class OpCodeHandler_VEX_VM : OpCodeHandlerModRM {
		readonly Register baseReg;
		readonly Code code;
		readonly MemorySize memSize;

		public OpCodeHandler_VEX_VM(Register baseReg, Code code, MemorySize memSize) {
			this.baseReg = baseReg;
			this.code = code;
			this.memSize = memSize;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.VEX);
			if ((int)state.vvvv != 0) {
				decoder.SetInvalidInstruction();
				return;
			}
			instruction.InternalCode = code;
			instruction.InternalOpCount = 2;
			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp0Kind = OpKind.Register;
			instruction.Op0Register = (int)state.reg + baseReg;
			if (state.mod == 3)
				decoder.SetInvalidInstruction();
			else {
				instruction.InternalOp2Kind = OpKind.Memory;
				instruction.InternalMemorySize = memSize;
				decoder.ReadOpMem_m32(ref instruction);
			}
		}
	}

	sealed class OpCodeHandler_VEX_MV : OpCodeHandlerModRM {
		readonly Register baseReg;
		readonly Code code;
		readonly MemorySize memSize;

		public OpCodeHandler_VEX_MV(Register baseReg, Code code, MemorySize memSize) {
			this.baseReg = baseReg;
			this.code = code;
			this.memSize = memSize;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.VEX);
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
				decoder.ReadOpMem_m32(ref instruction);
			}
			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp1Kind = OpKind.Register;
			instruction.Op1Register = (int)state.reg + baseReg;
		}
	}

	sealed class OpCodeHandler_VEX_M : OpCodeHandlerModRM {
		readonly Code code;
		readonly MemorySize memSize;

		public OpCodeHandler_VEX_M(Code code, MemorySize memSize) {
			this.code = code;
			this.memSize = memSize;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.VEX);
			if ((int)state.vvvv != 0) {
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
				decoder.ReadOpMem_m32(ref instruction);
			}
		}
	}

	sealed class OpCodeHandler_VEX_rDI_VX_RX : OpCodeHandlerModRM {
		readonly Register baseReg;
		readonly Code code;
		readonly MemorySize memSize;

		public OpCodeHandler_VEX_rDI_VX_RX(Register baseReg, Code code, MemorySize memSize) {
			this.baseReg = baseReg;
			this.code = code;
			this.memSize = memSize;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.VEX);
			if ((int)state.vvvv != 0) {
				decoder.SetInvalidInstruction();
				return;
			}
			instruction.InternalCode = code;
			instruction.InternalOpCount = 3;
			Debug.Assert(state.addressSize == OpSize.Size16 || state.addressSize == OpSize.Size32);
			if (state.addressSize == OpSize.Size32)
				instruction.InternalOp1Kind = OpKind.MemorySegEDI;
			else
				instruction.InternalOp1Kind = OpKind.MemorySegDI;
			instruction.InternalMemorySize = memSize;
			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp1Kind = OpKind.Register;
			instruction.Op1Register = (int)state.reg + baseReg;
			if (state.mod == 3) {
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp2Kind = OpKind.Register;
				instruction.Op2Register = (int)state.rm + baseReg;
			}
			else
				decoder.SetInvalidInstruction();
		}
	}

	sealed class OpCodeHandler_VEX_VWIb : OpCodeHandlerModRM {
		readonly Register baseReg1;
		readonly Register baseReg2;
		readonly Code code;
		readonly MemorySize memSize;

		public OpCodeHandler_VEX_VWIb(Register baseReg, Code code, MemorySize memSize) {
			baseReg1 = baseReg;
			baseReg2 = baseReg;
			this.code = code;
			this.memSize = memSize;
		}

		public OpCodeHandler_VEX_VWIb(Register baseReg, Code codeW0, Code codeW1, MemorySize memSize) {
			baseReg1 = baseReg;
			baseReg2 = baseReg;
			code = codeW0;
			this.memSize = memSize;
		}

		public OpCodeHandler_VEX_VWIb(Register baseReg1, Register baseReg2, Code code, MemorySize memSize) {
			this.baseReg1 = baseReg1;
			this.baseReg2 = baseReg2;
			this.code = code;
			this.memSize = memSize;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.VEX);
			if ((int)state.vvvv != 0) {
				decoder.SetInvalidInstruction();
				return;
			}
			instruction.InternalCode = code;
			instruction.InternalOpCount = 3;
			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp0Kind = OpKind.Register;
			instruction.Op0Register = (int)state.reg + baseReg1;
			if (state.mod == 3) {
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp1Kind = OpKind.Register;
				instruction.Op1Register = (int)state.rm + baseReg2;
			}
			else {
				instruction.InternalOp2Kind = OpKind.Memory;
				instruction.InternalMemorySize = memSize;
				decoder.ReadOpMem_m32(ref instruction);
			}
			instruction.InternalOp3Kind = OpKind.Immediate8;
			instruction.InternalImmediate8 = decoder.ReadByte();
		}
	}

	sealed class OpCodeHandler_VEX_WVIb : OpCodeHandlerModRM {
		readonly Register baseReg1;
		readonly Register baseReg2;
		readonly Code code;
		readonly MemorySize memSize;

		public OpCodeHandler_VEX_WVIb(Register baseReg, Code code, MemorySize memSize) {
			baseReg1 = baseReg;
			baseReg2 = baseReg;
			this.code = code;
			this.memSize = memSize;
		}

		public OpCodeHandler_VEX_WVIb(Register baseReg1, Register baseReg2, Code code, MemorySize memSize) {
			this.baseReg1 = baseReg1;
			this.baseReg2 = baseReg2;
			this.code = code;
			this.memSize = memSize;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.VEX);
			if ((int)state.vvvv != 0) {
				decoder.SetInvalidInstruction();
				return;
			}
			instruction.InternalCode = code;
			instruction.InternalOpCount = 3;
			if (state.mod == 3) {
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp0Kind = OpKind.Register;
				instruction.Op0Register = (int)state.rm + baseReg1;
			}
			else {
				instruction.InternalOp1Kind = OpKind.Memory;
				instruction.InternalMemorySize = memSize;
				decoder.ReadOpMem_m32(ref instruction);
			}
			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp1Kind = OpKind.Register;
			instruction.Op1Register = (int)state.reg + baseReg2;
			instruction.InternalOp3Kind = OpKind.Immediate8;
			instruction.InternalImmediate8 = decoder.ReadByte();
		}
	}

	sealed class OpCodeHandler_VEX_Ed_V_Ib : OpCodeHandlerModRM {
		readonly Register baseReg;
		readonly Code code32;
		readonly MemorySize memSize32;

		public OpCodeHandler_VEX_Ed_V_Ib(Register baseReg, Code code32, Code code64, MemorySize memSize32, MemorySize memSize64) {
			this.baseReg = baseReg;
			this.code32 = code32;
			this.memSize32 = memSize32;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.VEX);
			if ((int)state.vvvv != 0) {
				decoder.SetInvalidInstruction();
				return;
			}
			instruction.InternalCode = code32;
			instruction.InternalOpCount = 3;
			if (state.mod == 3) {
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp0Kind = OpKind.Register;
				instruction.Op0Register = (int)state.rm + Register.EAX;
			}
			else {
				instruction.InternalOp1Kind = OpKind.Memory;
				instruction.InternalMemorySize = memSize32;
				decoder.ReadOpMem_m32(ref instruction);
			}
			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp1Kind = OpKind.Register;
			instruction.Op1Register = (int)state.reg + baseReg;
			instruction.InternalOp3Kind = OpKind.Immediate8;
			instruction.InternalImmediate8 = decoder.ReadByte();
		}
	}

	sealed class OpCodeHandler_VEX_VHW : OpCodeHandlerModRM {
		readonly Register baseReg1;
		readonly Register baseReg2;
		readonly Register baseReg3;
		readonly Code codeR;
		readonly Code codeM;
		readonly MemorySize memSize;

		public OpCodeHandler_VEX_VHW(Register baseReg, Code codeR, Code codeM, MemorySize memSize) {
			baseReg1 = baseReg;
			baseReg2 = baseReg;
			baseReg3 = baseReg;
			this.codeR = codeR;
			this.codeM = codeM;
			this.memSize = memSize;
		}

		public OpCodeHandler_VEX_VHW(Register baseReg, Code code, MemorySize memSize) {
			baseReg1 = baseReg;
			baseReg2 = baseReg;
			baseReg3 = baseReg;
			codeR = code;
			codeM = code;
			this.memSize = memSize;
		}

		public OpCodeHandler_VEX_VHW(Register baseReg1, Register baseReg2, Register baseReg3, Code code, MemorySize memSize) {
			this.baseReg1 = baseReg1;
			this.baseReg2 = baseReg2;
			this.baseReg3 = baseReg3;
			codeR = code;
			codeM = code;
			this.memSize = memSize;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.VEX);
			instruction.InternalOpCount = 3;
			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp0Kind = OpKind.Register;
			instruction.Op0Register = (int)state.reg + baseReg1;
			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp1Kind = OpKind.Register;
			instruction.Op1Register = (int)state.vvvv + baseReg2;
			if (state.mod == 3) {
				instruction.InternalCode = codeR;
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp2Kind = OpKind.Register;
				instruction.Op2Register = (int)state.rm + baseReg3;
			}
			else {
				instruction.InternalCode = codeM;
				instruction.InternalOp3Kind = OpKind.Memory;
				instruction.InternalMemorySize = memSize;
				decoder.ReadOpMem_m32(ref instruction);
			}
		}
	}

	sealed class OpCodeHandler_VEX_WHV : OpCodeHandlerModRM {
		readonly Register baseReg;
		readonly Code codeR;
		readonly Code codeM;
		readonly MemorySize memSize;

		public OpCodeHandler_VEX_WHV(Register baseReg, Code codeR, Code codeM, MemorySize memSize) {
			this.baseReg = baseReg;
			this.codeR = codeR;
			this.codeM = codeM;
			this.memSize = memSize;
		}

		public OpCodeHandler_VEX_WHV(Register baseReg, Code code, MemorySize memSize) {
			this.baseReg = baseReg;
			codeR = code;
			codeM = code;
			this.memSize = memSize;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.VEX);
			instruction.InternalOpCount = 3;
			if (state.mod == 3) {
				instruction.InternalCode = codeR;
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp0Kind = OpKind.Register;
				instruction.Op0Register = (int)state.rm + baseReg;
			}
			else {
				instruction.InternalCode = codeM;
				instruction.InternalOp1Kind = OpKind.Memory;
				instruction.InternalMemorySize = memSize;
				decoder.ReadOpMem_m32(ref instruction);
			}
			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp1Kind = OpKind.Register;
			instruction.Op1Register = (int)state.vvvv + baseReg;
			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp2Kind = OpKind.Register;
			instruction.Op2Register = (int)state.reg + baseReg;
		}
	}

	sealed class OpCodeHandler_VEX_VHM : OpCodeHandlerModRM {
		readonly Register baseReg;
		readonly Code code;
		readonly MemorySize memSize;

		public OpCodeHandler_VEX_VHM(Register baseReg, Code code, MemorySize memSize) {
			this.baseReg = baseReg;
			this.code = code;
			this.memSize = memSize;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.VEX);
			instruction.InternalCode = code;
			instruction.InternalOpCount = 3;
			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp0Kind = OpKind.Register;
			instruction.Op0Register = (int)state.reg + baseReg;
			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp1Kind = OpKind.Register;
			instruction.Op1Register = (int)state.vvvv + baseReg;
			if (state.mod == 3)
				decoder.SetInvalidInstruction();
			else {
				instruction.InternalOp3Kind = OpKind.Memory;
				instruction.InternalMemorySize = memSize;
				decoder.ReadOpMem_m32(ref instruction);
			}
		}
	}

	sealed class OpCodeHandler_VEX_MHV : OpCodeHandlerModRM {
		readonly Register baseReg;
		readonly Code code;
		readonly MemorySize memSize;

		public OpCodeHandler_VEX_MHV(Register baseReg, Code code, MemorySize memSize) {
			this.baseReg = baseReg;
			this.code = code;
			this.memSize = memSize;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.VEX);
			instruction.InternalCode = code;
			instruction.InternalOpCount = 3;
			if (state.mod == 3)
				decoder.SetInvalidInstruction();
			else {
				instruction.InternalOp1Kind = OpKind.Memory;
				instruction.InternalMemorySize = memSize;
				decoder.ReadOpMem_m32(ref instruction);
			}
			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp1Kind = OpKind.Register;
			instruction.Op1Register = (int)state.vvvv + baseReg;
			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp2Kind = OpKind.Register;
			instruction.Op2Register = (int)state.reg + baseReg;
		}
	}

	sealed class OpCodeHandler_VEX_VHWIb : OpCodeHandlerModRM {
		readonly Register baseReg1;
		readonly Register baseReg2;
		readonly Register baseReg3;
		readonly Code code;
		readonly MemorySize memSize;

		public OpCodeHandler_VEX_VHWIb(Register baseReg, Code code, MemorySize memSize) {
			baseReg1 = baseReg;
			baseReg2 = baseReg;
			baseReg3 = baseReg;
			this.code = code;
			this.memSize = memSize;
		}

		public OpCodeHandler_VEX_VHWIb(Register baseReg1, Register baseReg2, Register baseReg3, Code code, MemorySize memSize) {
			this.baseReg1 = baseReg1;
			this.baseReg2 = baseReg2;
			this.baseReg3 = baseReg3;
			this.code = code;
			this.memSize = memSize;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.VEX);
			instruction.InternalCode = code;
			instruction.InternalOpCount = 4;
			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp0Kind = OpKind.Register;
			instruction.Op0Register = (int)state.reg + baseReg1;
			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp1Kind = OpKind.Register;
			instruction.Op1Register = (int)state.vvvv + baseReg2;
			if (state.mod == 3) {
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp2Kind = OpKind.Register;
				instruction.Op2Register = (int)state.rm + baseReg3;
			}
			else {
				instruction.InternalOp3Kind = OpKind.Memory;
				instruction.InternalMemorySize = memSize;
				decoder.ReadOpMem_m32(ref instruction);
			}
			instruction.InternalOp4Kind = OpKind.Immediate8;
			instruction.InternalImmediate8 = decoder.ReadByte();
		}
	}

	sealed class OpCodeHandler_VEX_HRIb : OpCodeHandlerModRM {
		readonly Register baseReg;
		readonly Code code;

		public OpCodeHandler_VEX_HRIb(Register baseReg, Code code) {
			this.baseReg = baseReg;
			this.code = code;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.VEX);
			instruction.InternalCode = code;
			instruction.InternalOpCount = 3;
			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp0Kind = OpKind.Register;
			instruction.Op0Register = (int)state.vvvv + baseReg;
			if (state.mod == 3) {
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp1Kind = OpKind.Register;
				instruction.Op1Register = (int)state.rm + baseReg;
			}
			else
				decoder.SetInvalidInstruction();
			instruction.InternalOp3Kind = OpKind.Immediate8;
			instruction.InternalImmediate8 = decoder.ReadByte();
		}
	}

	sealed class OpCodeHandler_VEX_VHWIs4 : OpCodeHandlerModRM {
		readonly Register baseReg;
		readonly Code code;
		readonly MemorySize memSize;

		public OpCodeHandler_VEX_VHWIs4(Register baseReg, Code code, MemorySize memSize) {
			this.baseReg = baseReg;
			this.code = code;
			this.memSize = memSize;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.VEX);
			instruction.InternalCode = code;
			instruction.InternalOpCount = 4;
			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp0Kind = OpKind.Register;
			instruction.Op0Register = (int)state.reg + baseReg;
			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp1Kind = OpKind.Register;
			instruction.Op1Register = (int)state.vvvv + baseReg;
			if (state.mod == 3) {
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp2Kind = OpKind.Register;
				instruction.Op2Register = (int)state.rm + baseReg;
			}
			else {
				instruction.InternalOp3Kind = OpKind.Memory;
				instruction.InternalMemorySize = memSize;
				decoder.ReadOpMem_m32(ref instruction);
			}
			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp3Kind = OpKind.Register;
			instruction.Op3Register = (int)((decoder.ReadByte() >> 4) & 7) + baseReg;
		}
	}

	sealed class OpCodeHandler_VEX_VK_HK_RK : OpCodeHandlerModRM {
		readonly Code code;

		public OpCodeHandler_VEX_VK_HK_RK(Code code) => this.code = code;

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.VEX);
			if ((int)state.vvvv > 7) {
				decoder.SetInvalidInstruction();
				return;
			}
			instruction.InternalCode = code;
			instruction.InternalOpCount = 3;
			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp0Kind = OpKind.Register;
			instruction.Op0Register = (int)state.reg + Register.K0;
			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp1Kind = OpKind.Register;
			instruction.Op1Register = (int)state.vvvv + Register.K0;// vvvv is valid, see above
			if (state.mod == 3) {
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp2Kind = OpKind.Register;
				instruction.Op2Register = (int)state.rm + Register.K0;
			}
			else
				decoder.SetInvalidInstruction();
		}
	}

	sealed class OpCodeHandler_VEX_VK_RK : OpCodeHandlerModRM {
		readonly Code code;

		public OpCodeHandler_VEX_VK_RK(Code code) => this.code = code;

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.VEX);
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
				instruction.Op1Register = (int)state.rm + Register.K0;
			}
			else
				decoder.SetInvalidInstruction();
		}
	}

	sealed class OpCodeHandler_VEX_VK_RK_Ib : OpCodeHandlerModRM {
		readonly Code code;

		public OpCodeHandler_VEX_VK_RK_Ib(Code code) => this.code = code;

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.VEX);
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
				instruction.Op1Register = (int)state.rm + Register.K0;
			}
			else
				decoder.SetInvalidInstruction();
			instruction.InternalOp3Kind = OpKind.Immediate8;
			instruction.InternalImmediate8 = decoder.ReadByte();
		}
	}

	sealed class OpCodeHandler_VEX_VK_WK : OpCodeHandlerModRM {
		readonly Code code;
		readonly MemorySize memSize;

		public OpCodeHandler_VEX_VK_WK(Code code, MemorySize memSize) {
			this.code = code;
			this.memSize = memSize;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.VEX);
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
				instruction.Op1Register = (int)state.rm + Register.K0;
			}
			else {
				instruction.InternalOp2Kind = OpKind.Memory;
				instruction.InternalMemorySize = memSize;
				decoder.ReadOpMem_m32(ref instruction);
			}
		}
	}

	sealed class OpCodeHandler_VEX_MK_VK : OpCodeHandlerModRM {
		readonly Code code;
		readonly MemorySize memSize;

		public OpCodeHandler_VEX_MK_VK(Code code, MemorySize memSize) {
			this.code = code;
			this.memSize = memSize;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.VEX);
			if ((int)state.vvvv != 0) {
				decoder.SetInvalidInstruction();
				return;
			}
			instruction.InternalCode = code;
			instruction.InternalOpCount = 2;
			if (state.mod == 3) {
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp0Kind = OpKind.Register;
				instruction.Op0Register = (int)state.rm + Register.K0;
			}
			else {
				instruction.InternalOp1Kind = OpKind.Memory;
				instruction.InternalMemorySize = memSize;
				decoder.ReadOpMem_m32(ref instruction);
			}
			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp1Kind = OpKind.Register;
			instruction.Op1Register = (int)state.reg + Register.K0;
		}
	}

	sealed class OpCodeHandler_VEX_VK_R : OpCodeHandlerModRM {
		readonly Code code;
		readonly Register gpr;

		public OpCodeHandler_VEX_VK_R(Code code, Register gpr) {
			this.code = code;
			this.gpr = gpr;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.VEX);
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
				instruction.Op1Register = (int)state.rm + gpr;
			}
			else
				decoder.SetInvalidInstruction();
		}
	}

	sealed class OpCodeHandler_VEX_G_VK : OpCodeHandlerModRM {
		readonly Code code;
		readonly Register gpr;

		public OpCodeHandler_VEX_G_VK(Code code, Register gpr) {
			this.code = code;
			this.gpr = gpr;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.VEX);
			if ((int)state.vvvv != 0) {
				decoder.SetInvalidInstruction();
				return;
			}
			instruction.InternalCode = code;
			instruction.InternalOpCount = 2;
			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp0Kind = OpKind.Register;
			instruction.Op0Register = (int)state.reg + gpr;
			if (state.mod == 3) {
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp1Kind = OpKind.Register;
				instruction.Op1Register = (int)state.rm + Register.K0;
			}
			else
				decoder.SetInvalidInstruction();
		}
	}

	sealed class OpCodeHandler_VEX_Gv_W : OpCodeHandlerModRM {
		readonly Register baseReg;
		readonly Code codeW0;
		readonly MemorySize memSize;

		public OpCodeHandler_VEX_Gv_W(Register baseReg, Code codeW0, Code codeW1, MemorySize memSize) {
			this.baseReg = baseReg;
			this.codeW0 = codeW0;
			this.memSize = memSize;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.VEX);
			if ((int)state.vvvv != 0) {
				decoder.SetInvalidInstruction();
				return;
			}
			instruction.InternalCode = codeW0;
			instruction.InternalOpCount = 2;
			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp0Kind = OpKind.Register;
			instruction.Op0Register = (int)state.reg + Register.EAX;
			if (state.mod == 3) {
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp1Kind = OpKind.Register;
				instruction.Op1Register = (int)state.rm + baseReg;
			}
			else {
				instruction.InternalOp2Kind = OpKind.Memory;
				instruction.InternalMemorySize = memSize;
				decoder.ReadOpMem_m32(ref instruction);
			}
		}
	}

	sealed class OpCodeHandler_VEX_Gv_RX : OpCodeHandlerModRM {
		readonly Register baseReg;
		readonly Code code32;

		public OpCodeHandler_VEX_Gv_RX(Register baseReg, Code code32, Code code64) {
			this.baseReg = baseReg;
			this.code32 = code32;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.VEX);
			if ((int)state.vvvv != 0) {
				decoder.SetInvalidInstruction();
				return;
			}
			instruction.InternalCode = code32;
			instruction.InternalOpCount = 2;
			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp0Kind = OpKind.Register;
			instruction.Op0Register = (int)state.reg + Register.EAX;
			if (state.mod == 3) {
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp1Kind = OpKind.Register;
				instruction.Op1Register = (int)state.rm + baseReg;
			}
			else
				decoder.SetInvalidInstruction();
		}
	}

	sealed class OpCodeHandler_VEX_Gv_GPR_Ib : OpCodeHandlerModRM {
		readonly Register baseReg;
		readonly Code code32;

		public OpCodeHandler_VEX_Gv_GPR_Ib(Register baseReg, Code code32, Code code64) {
			this.baseReg = baseReg;
			this.code32 = code32;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.VEX);
			if ((int)state.vvvv != 0) {
				decoder.SetInvalidInstruction();
				return;
			}
			instruction.InternalCode = code32;
			instruction.InternalOpCount = 3;
			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp0Kind = OpKind.Register;
			instruction.Op0Register = (int)state.reg + Register.EAX;
			if (state.mod == 3) {
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp1Kind = OpKind.Register;
				instruction.Op1Register = (int)state.rm + baseReg;
			}
			else
				decoder.SetInvalidInstruction();
			instruction.InternalOp3Kind = OpKind.Immediate8;
			instruction.InternalImmediate8 = decoder.ReadByte();
		}
	}

	sealed class OpCodeHandler_VEX_VX_VSIB_HX : OpCodeHandlerModRM {
		readonly Register baseReg1;
		readonly Register vsibIndex;
		readonly Register baseReg3;
		readonly Code code;
		readonly MemorySize memSize;

		public OpCodeHandler_VEX_VX_VSIB_HX(Register baseReg1, Register vsibIndex, Register baseReg3, Code code, MemorySize memSize) {
			this.baseReg1 = baseReg1;
			this.vsibIndex = vsibIndex;
			this.baseReg3 = baseReg3;
			this.code = code;
			this.memSize = memSize;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.VEX);
			instruction.InternalCode = code;
			instruction.InternalOpCount = 3;
			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp0Kind = OpKind.Register;
			instruction.Op0Register = (int)state.reg + baseReg1;
			if (state.mod == 3)
				decoder.SetInvalidInstruction();
			else {
				instruction.InternalOp2Kind = OpKind.Memory;
				instruction.InternalMemorySize = memSize;
				decoder.ReadOpMem_VSIB_m32(ref instruction, vsibIndex);
			}
			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp2Kind = OpKind.Register;
			instruction.Op2Register = (int)state.vvvv + baseReg3;
		}
	}

	sealed class OpCodeHandler_VEX_Gv_Gv_Ev : OpCodeHandlerModRM {
		readonly Code code32;

		public OpCodeHandler_VEX_Gv_Gv_Ev(Code code32, Code code64) {
			this.code32 = code32;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.VEX);
			instruction.InternalCode = code32;
			instruction.InternalOpCount = 3;
			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp0Kind = OpKind.Register;
			instruction.Op0Register = (int)state.reg + Register.EAX;
			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp1Kind = OpKind.Register;
			instruction.Op1Register = (int)state.vvvv + Register.EAX;
			if (state.mod == 3) {
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp2Kind = OpKind.Register;
				instruction.Op2Register = (int)state.rm + Register.EAX;
			}
			else {
				instruction.InternalOp3Kind = OpKind.Memory;
				instruction.InternalMemorySize = MemorySize.UInt32;
				decoder.ReadOpMem_m32(ref instruction);
			}
		}
	}

	sealed class OpCodeHandler_VEX_Gv_Ev_Gv : OpCodeHandlerModRM {
		readonly Code code32;
		readonly MemorySize memSize32;

		public OpCodeHandler_VEX_Gv_Ev_Gv(Code code32, Code code64) {
			this.code32 = code32;
			memSize32 = MemorySize.UInt32;
		}

		public OpCodeHandler_VEX_Gv_Ev_Gv(Code code32, Code code64, MemorySize memSize32, MemorySize memSize64) {
			this.code32 = code32;
			this.memSize32 = memSize32;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.VEX);
			instruction.InternalCode = code32;
			instruction.InternalOpCount = 3;
			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp0Kind = OpKind.Register;
			instruction.Op0Register = (int)state.reg + Register.EAX;
			if (state.mod == 3) {
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp1Kind = OpKind.Register;
				instruction.Op1Register = (int)state.rm + Register.EAX;
			}
			else {
				instruction.InternalOp2Kind = OpKind.Memory;
				instruction.InternalMemorySize = memSize32;
				decoder.ReadOpMem_m32(ref instruction);
			}
			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp2Kind = OpKind.Register;
			instruction.Op2Register = (int)state.vvvv + Register.EAX;
		}
	}

	sealed class OpCodeHandler_VEX_Gv_Ev : OpCodeHandlerModRM {
		readonly Code code32;

		public OpCodeHandler_VEX_Gv_Ev(Code code32, Code code64) => this.code32 = code32;

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.VEX);
			instruction.InternalCode = code32;
			instruction.InternalOpCount = 2;
			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp0Kind = OpKind.Register;
			instruction.Op0Register = (int)(int)state.vvvv + Register.EAX;
			if (state.mod == 3) {
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp1Kind = OpKind.Register;
				instruction.Op1Register = (int)state.rm + Register.EAX;
			}
			else {
				instruction.InternalOp2Kind = OpKind.Memory;
				instruction.InternalMemorySize = MemorySize.UInt32;
				decoder.ReadOpMem_m32(ref instruction);
			}
		}
	}

	sealed class OpCodeHandler_VEX_GvM_VX_Ib : OpCodeHandlerModRM {
		readonly Register baseReg;
		readonly Code code32;
		readonly MemorySize memSize32;

		public OpCodeHandler_VEX_GvM_VX_Ib(Register baseReg, Code code32, Code code64, MemorySize memSize32, MemorySize memSize64) {
			this.baseReg = baseReg;
			this.code32 = code32;
			this.memSize32 = memSize32;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.VEX);
			if ((int)state.vvvv != 0) {
				decoder.SetInvalidInstruction();
				return;
			}
			instruction.InternalCode = code32;
			instruction.InternalOpCount = 3;
			if (state.mod == 3) {
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp0Kind = OpKind.Register;
				instruction.Op0Register = (int)state.rm + Register.EAX;
			}
			else {
				instruction.InternalOp1Kind = OpKind.Memory;
				instruction.InternalMemorySize = memSize32;
				decoder.ReadOpMem_m32(ref instruction);
			}
			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp1Kind = OpKind.Register;
			instruction.Op1Register = (int)state.reg + baseReg;
			instruction.InternalOp3Kind = OpKind.Immediate8;
			instruction.InternalImmediate8 = decoder.ReadByte();
		}
	}

	sealed class OpCodeHandler_VEX_Gv_Ev_Ib : OpCodeHandlerModRM {
		readonly Code code32;

		public OpCodeHandler_VEX_Gv_Ev_Ib(Code code32, Code code64) => this.code32 = code32;

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.VEX);
			if ((int)state.vvvv != 0) {
				decoder.SetInvalidInstruction();
				return;
			}
			instruction.InternalCode = code32;
			instruction.InternalOpCount = 3;
			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp0Kind = OpKind.Register;
			instruction.Op0Register = (int)state.reg + Register.EAX;
			if (state.mod == 3) {
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp1Kind = OpKind.Register;
				instruction.Op1Register = (int)state.rm + Register.EAX;
			}
			else {
				instruction.InternalOp2Kind = OpKind.Memory;
				instruction.InternalMemorySize = MemorySize.UInt32;
				decoder.ReadOpMem_m32(ref instruction);
			}
			instruction.InternalOp3Kind = OpKind.Immediate8;
			instruction.InternalImmediate8 = decoder.ReadByte();
		}
	}
}
#endif

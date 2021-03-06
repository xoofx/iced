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

#if !NO_NASM_FORMATTER && !NO_FORMATTER
using System.Collections.Generic;
using Xunit;

namespace Iced.UnitTests.Intel.FormatterTests.Nasm {
	public sealed class NasmFormatterTest32_NoMemSize_000 : FormatterTest {
		[Theory]
		[MemberData(nameof(Format_Data))]
		void Format(int index, InstructionInfo info, string formattedString) => FormatBase(index, info, formattedString, NasmFormatterFactory.Create_NoMemSize());
		public static IEnumerable<object[]> Format_Data => GetFormatData(infos, formattedStrings);

		static readonly InstructionInfo[] infos = InstructionInfos32_000.AllInfos;
		static readonly string[] formattedStrings = new string[InstructionInfos32_000.AllInfos_Length] {
			"o16 push es",
			"push es",
			"o16 pop es",
			"pop es",
			"o16 push cs",
			"push cs",
			"o16 push ss",
			"push ss",
			"o16 pop ss",
			"pop ss",
			"o16 push ds",
			"push ds",
			"o16 pop ds",
			"pop ds",
			"daa",
			"das",
			"aaa",
			"aas",
			"inc ax",
			"inc eax",
			"inc cx",
			"inc ecx",
			"inc dx",
			"inc edx",
			"inc bx",
			"inc ebx",
			"inc sp",
			"inc esp",
			"inc bp",
			"inc ebp",
			"inc si",
			"inc esi",
			"inc di",
			"inc edi",
			"dec ax",
			"dec eax",
			"dec cx",
			"dec ecx",
			"dec dx",
			"dec edx",
			"dec bx",
			"dec ebx",
			"dec sp",
			"dec esp",
			"dec bp",
			"dec ebp",
			"dec si",
			"dec esi",
			"dec di",
			"dec edi",
			"push eax",
			"push ecx",
			"push edx",
			"push ebx",
			"push esp",
			"push ebp",
			"push esi",
			"push edi",
			"pop eax",
			"pop ecx",
			"pop edx",
			"pop ebx",
			"pop esp",
			"pop ebp",
			"pop esi",
			"pop edi",
			"pushaw",
			"pusha",
			"popaw",
			"popa",
			"bound bx,[eax]",
			"bound ebx,[eax]",
			"arpl dx,si",
			"arpl [eax],bx",
			"push dword 0x3412a55a",
			"push byte 0xffffffa5",
			"o16 jo 0x004d",
			"jo 0x8000004c",
			"o16 jno 0x004d",
			"jno 0x8000004c",
			"o16 jb 0x004d",
			"jb 0x8000004c",
			"o16 jae 0x004d",
			"jae 0x8000004c",
			"o16 je 0x004d",
			"je 0x8000004c",
			"o16 jne 0x004d",
			"jne 0x8000004c",
			"o16 jbe 0x004d",
			"jbe 0x8000004c",
			"o16 ja 0x004d",
			"ja 0x8000004c",
			"o16 js 0x004d",
			"js 0x8000004c",
			"o16 jns 0x004d",
			"jns 0x8000004c",
			"o16 jp 0x004d",
			"jp 0x8000004c",
			"o16 jnp 0x004d",
			"jnp 0x8000004c",
			"o16 jl 0x004d",
			"jl 0x8000004c",
			"o16 jge 0x004d",
			"jge 0x8000004c",
			"o16 jle 0x004d",
			"jle 0x8000004c",
			"o16 jg 0x004d",
			"jg 0x8000004c",
			"pop esi",
			"pop dword [eax]",
			"call 0x7856:0x3412",
			"call 0xbc9a:0x78563412",
			"pushf",
			"popf",
			"retw 0xa55a",
			"ret 0xa55a",
			"retw",
			"ret",
			"les bx,[eax]",
			"les ebx,[eax]",
			"lds bx,[eax]",
			"lds ebx,[eax]",
			"enter 0xa55a,0xa6",
			"leave",
			"into",
			"aam",
			"aad",
			"o16 loopne 0x004e,cx",
			"loopne 0x8000004d,cx",
			"o16 loopne 0x004d",
			"loopne 0x8000004c",
			"o16 loope 0x004e,cx",
			"loope 0x8000004d,cx",
			"o16 loope 0x004d",
			"loope 0x8000004c",
			"o16 loop 0x004e,cx",
			"loop 0x8000004d,cx",
			"o16 loop 0x004d",
			"loop 0x8000004c",
			"o16 jcxz 0x004e",
			"jcxz 0x8000004d",
			"o16 jecxz 0x004d",
			"jecxz 0x8000004c",
			"call 0xa54e",
			"call 0x255a3407",
			"jmp 0xa54e",
			"jmp 0x255a3407",
			"jmp 0x7856:0x3412",
			"jmp 0xbcea:0x78563412",
			"o16 jmp 0x004d",
			"jmp 0x8000004c",
			"call cx",
			"call [eax]",
			"call ecx",
			"call [eax]",
			"jmp dx",
			"jmp [eax]",
			"jmp edx",
			"jmp [eax]",
			"push esi",
			"push dword [eax]",
			"bndmov bnd1,bnd2",
			"bndmov bnd1,[eax]",
			"bndcl bnd1,edx",
			"bndcl bnd1,[eax]",
			"bndcu bnd1,edx",
			"bndcu bnd1,[eax]",
			"bndmov bnd2,bnd1",
			"bndmov [eax],bnd1",
			"bndmk bnd1,[eax]",
			"bndcn bnd1,edx",
			"bndcn bnd1,[eax]",
			"mov esi,cr3",
			"mov esi,dr3",
			"mov cr3,esi",
			"mov dr3,esi",
			"vmread esi,ecx",
			"vmread [eax],ebx",
			"vmwrite ecx,esi",
			"vmwrite ebx,[eax]",
			"jo 0xa54f",
			"jo 0xb412a550",
			"jno 0xa54f",
			"jno 0xb412a550",
			"jb 0xa54f",
			"jb 0xb412a550",
			"jae 0xa54f",
			"jae 0xb412a550",
			"je 0xa54f",
			"je 0xb412a550",
			"jne 0xa54f",
			"jne 0xb412a550",
			"jbe 0xa54f",
			"jbe 0xb412a550",
			"ja 0xa54f",
			"ja 0xb412a550",
			"js 0xa54f",
			"js 0xb412a550",
			"jns 0xa54f",
			"jns 0xb412a550",
			"jp 0xa54f",
			"jp 0xb412a550",
			"jnp 0xa54f",
			"jnp 0xb412a550",
			"jl 0xa54f",
			"jl 0xb412a550",
			"jge 0xa54f",
			"jge 0xb412a550",
			"jle 0xa54f",
			"jle 0xb412a550",
			"jg 0xa54f",
			"jg 0xb412a550",
			"push fs",
			"pop fs",
			"push gs",
			"pop gs",
			"rdpid edx",
			"invept edx,[eax]",
			"invvpid edx,[eax]",
			"invpcid edx,[eax]",
			"salc",
			"push word 0xa55a",
			"o16 push byte 0xffa5",
			"a16 insb",
			"insb",
			"a16 insw",
			"insw",
			"a16 insd",
			"insd",
			"a16 outsb",
			"outsb",
			"a16 outsw",
			"outsw",
			"a16 outsd",
			"outsd",
			"mov fs,si",
			"o16 mov ds,[eax]",
			"mov fs,esi",
			"mov ds,[eax]",
			"pop si",
			"pop word [eax]",
			"xchg ax,ax",
			"nop",
			"pushfw",
			"popfw",
			"mov al,[0xf0debc9a]",
			"mov al,[word 0xf0de]",
			"mov ax,[0xf0debc9a]",
			"mov ax,[word 0xf0de]",
			"mov eax,[0xf0debc9a]",
			"mov eax,[word 0xf0de]",
			"mov [0xf0debc9a],al",
			"mov [word 0xf0de],al",
			"mov [0xf0debc9a],ax",
			"mov [word 0xf0de],ax",
			"mov [0xf0debc9a],eax",
			"mov [word 0xf0de],eax",
			"a16 movsb",
			"movsb",
			"a16 movsw",
			"movsw",
			"a16 movsd",
			"movsd",
			"a16 cmpsb",
			"cmpsb",
			"a16 cmpsw",
			"cmpsw",
			"a16 cmpsd",
			"cmpsd",
			"a16 stosb",
			"stosb",
			"a16 stosw",
			"stosw",
			"a16 stosd",
			"stosd",
			"a16 lodsb",
			"lodsb",
			"a16 lodsw",
			"lodsw",
			"a16 lodsd",
			"lodsd",
			"a16 scasb",
			"scasb",
			"a16 scasw",
			"scasw",
			"a16 scasd",
			"scasd",
			"xbegin 0x7fffa54f",
			"xbegin 0xb412a550",
			"o16 enter 0xa55a,0xa6",
			"o16 leave",
			"retfw 0xa55a",
			"retf 0xa55a",
			"retfw",
			"retf",
			"iretw",
			"iret",
			"rol cl,cl",
			"rol byte [eax],cl",
			"ror dl,cl",
			"ror byte [eax],cl",
			"rcl bl,cl",
			"rcl byte [eax],cl",
			"rcr ah,cl",
			"rcr byte [eax],cl",
			"shl ch,cl",
			"shl byte [eax],cl",
			"shr dh,cl",
			"shr byte [eax],cl",
			"sar al,cl",
			"sar byte [eax],cl",
			"rol cx,cl",
			"rol word [eax],cl",
			"rol ecx,cl",
			"rol dword [eax],cl",
			"ror dx,cl",
			"ror word [eax],cl",
			"ror edx,cl",
			"ror dword [eax],cl",
			"rcl bx,cl",
			"rcl word [eax],cl",
			"rcl ebx,cl",
			"rcl dword [eax],cl",
			"rcr sp,cl",
			"rcr word [eax],cl",
			"rcr esp,cl",
			"rcr dword [eax],cl",
			"shl bp,cl",
			"shl word [eax],cl",
			"shl ebp,cl",
			"shl dword [eax],cl",
			"shr si,cl",
			"shr word [eax],cl",
			"shr esi,cl",
			"shr dword [eax],cl",
			"sar ax,cl",
			"sar word [eax],cl",
			"sar eax,cl",
			"sar dword [eax],cl",
			"fldcw [eax]",
			"fnstcw [eax]",
			"fnstsw [eax]",
			"call word far [eax]",
			"call far [eax]",
			"jmp word far [eax]",
			"jmp far [eax]",
			"push si",
			"push word [eax]",
			"o16 sgdt [eax]",
			"sgdt [eax]",
			"o16 sidt [eax]",
			"sidt [eax]",
			"o16 lgdt [eax]",
			"lgdt [eax]",
			"o16 lidt [eax]",
			"lidt [eax]",
			"lar cx,si",
			"lar bx,[eax]",
			"lar ecx,esi",
			"lar ebx,[eax]",
			"lsl cx,si",
			"lsl bx,[eax]",
			"lsl ecx,esi",
			"lsl ebx,[eax]",
			"bndldx bnd1,[eax]",
			"bndstx [eax],bnd1",
			"kmovw k2,k3",
			"kmovw k1,[eax]",
			"kmovb k2,k3",
			"kmovb k1,[eax]",
			"kmovd k2,k3",
			"kmovd k1,[eax]",
			"kmovw [eax],k1",
			"kmovb [eax],k1",
			"kmovd [eax],k1",
			"kmovw k2,ebx",
			"kmovb k2,ebx",
			"kmovd k2,ebx",
			"kmovw edx,k3",
			"kmovb edx,k3",
			"kmovd edx,k3",
			"kortestw k2,k3",
			"kortestq k2,k3",
			"kortestb k2,k3",
			"kortestd k2,k3",
			"ktestw k2,k3",
			"ktestq k2,k3",
			"ktestb k2,k3",
			"ktestd k2,k3",
			"o16 push fs",
			"o16 pop fs",
			"shld si,cx,cl",
			"shld [eax],bx,cl",
			"shld esi,ecx,cl",
			"shld [eax],ebx,cl",
			"o16 push gs",
			"o16 pop gs",
			"shrd si,cx,cl",
			"shrd [eax],bx,cl",
			"shrd esi,ecx,cl",
			"shrd [eax],ebx,cl",
			"lss bx,[eax]",
			"lss ebx,[eax]",
			"lfs bx,[eax]",
			"lfs ebx,[eax]",
			"lgs bx,[eax]",
			"lgs ebx,[eax]",
			"movzx cx,dh",
			"movzx bx,byte [eax]",
			"movzx ecx,dh",
			"movzx ebx,byte [eax]",
			"movzx cx,si",
			"movzx bx,word [eax]",
			"movzx ecx,si",
			"movzx ebx,word [eax]",
			"movsx cx,dh",
			"movsx bx,byte [eax]",
			"movsx ecx,dh",
			"movsx ebx,byte [eax]",
			"movsx cx,si",
			"movsx bx,word [eax]",
			"movsx ecx,si",
			"movsx ebx,word [eax]",
			"cmpxchg8b [eax]",
			"crc32 ecx,dh",
			"crc32 ebx,byte [eax]",
			"crc32 ecx,esi",
			"crc32 ebx,dword [eax]",
			"a16 maskmovq mm2,mm3",
			"maskmovq mm2,mm3",
			"a16 maskmovdqu xmm2,xmm3",
			"maskmovdqu xmm2,xmm3",
			"a16 vmaskmovdqu xmm2,xmm3",
			"vmaskmovdqu xmm2,xmm3",
			"fs outsb",
			"fs outsw",
			"fs outsd",
			"fs movsb",
			"fs movsw",
			"fs movsd",
			"fs cmpsb",
			"fs cmpsw",
			"fs cmpsd",
			"fs lodsb",
			"fs lodsw",
			"fs lodsd",
			"a16 xlatb",
			"fs xlatb",
			"fs maskmovq mm2,mm3",
			"fs maskmovdqu xmm2,xmm3",
			"fs vmaskmovdqu xmm2,xmm3",
			"xlatb",
			"aam 0xfa",
			"aad 0xfa",
			"o16 fldenv [eax]",
			"fldenv [eax]",
			"o16 fnstenv [eax]",
			"fnstenv [eax]",
			"o16 frstor [eax]",
			"frstor [eax]",
			"o16 fnsave [eax]",
			"fnsave [eax]",
			"a16 monitor",
			"monitor",
			"mwait",
			"mov al,[fs:0xf0debc9a]",
			"mov al,[word fs:0xf0de]",
			"mov ax,[fs:0xf0debc9a]",
			"mov ax,[word fs:0xf0de]",
			"mov eax,[fs:0xf0debc9a]",
			"mov eax,[word fs:0xf0de]",
			"mov [fs:0xf0debc9a],al",
			"mov [word fs:0xf0de],al",
			"mov [fs:0xf0debc9a],ax",
			"mov [word fs:0xf0de],ax",
			"mov [fs:0xf0debc9a],eax",
			"mov [word fs:0xf0de],eax",
			"mov cl,[eax]",
			"mov cl,[eax-0x12]",
			"mov cl,[eax-0x12345678]",
			"mov cl,[0xedcba988]",
			"mov cl,[eax+0x12]",
			"mov cl,[eax+0x12345678]",
			"mov cl,[0x12345678]",
			"mov cl,[eax+ecx]",
			"mov cl,[eax+ecx*2]",
			"mov cl,[eax+ecx*4]",
			"mov cl,[eax+ecx*8]",
			"mov cl,[eax+ecx-0x12]",
			"mov cl,[eax+ecx*2-0x12]",
			"mov cl,[eax+ecx*4-0x12]",
			"mov cl,[eax+ecx*8-0x12]",
			"mov cl,[eax+ecx+0x12]",
			"mov cl,[eax+ecx*2+0x12]",
			"mov cl,[eax+ecx*4+0x12]",
			"mov cl,[eax+ecx*8+0x12]",
			"mov cl,[eax+ecx-0x12345678]",
			"mov cl,[eax+ecx*2-0x12345678]",
			"mov cl,[eax+ecx*4-0x12345678]",
			"mov cl,[eax+ecx*8-0x12345678]",
			"mov cl,[eax+ecx+0x12345678]",
			"mov cl,[eax+ecx*2+0x12345678]",
			"mov cl,[eax+ecx*4+0x12345678]",
			"mov cl,[eax+ecx*8+0x12345678]",
			"mov cl,[ecx*1-0x12345678]",
			"mov cl,[ebp+ecx*2-0x12]",
			"mov cl,[ebp+ecx*4-0x12345678]",
			"mov cl,[ecx*8-0x12345678]",
			"mov cl,[ecx*1+0x12345678]",
			"mov cl,[ebp+ecx*2+0x12]",
			"mov cl,[ebp+ecx*4+0x12345678]",
			"mov cl,[ecx*8+0x12345678]",
			"mov cl,[fs:eax]",
			"mov cl,[fs:eax-0x12]",
			"mov cl,[fs:eax-0x12345678]",
			"mov cl,[fs:0xedcba988]",
			"mov cl,[fs:eax+0x12]",
			"mov cl,[fs:eax+0x12345678]",
			"mov cl,[fs:0x12345678]",
			"mov cl,[fs:eax+ecx]",
			"mov cl,[fs:eax+ecx*2]",
			"mov cl,[fs:eax+ecx*4]",
			"mov cl,[fs:eax+ecx*8]",
			"mov cl,[fs:eax+ecx-0x12]",
			"mov cl,[fs:eax+ecx*2-0x12]",
			"mov cl,[fs:eax+ecx*4-0x12]",
			"mov cl,[fs:eax+ecx*8-0x12]",
			"mov cl,[fs:eax+ecx+0x12]",
			"mov cl,[fs:eax+ecx*2+0x12]",
			"mov cl,[fs:eax+ecx*4+0x12]",
			"mov cl,[fs:eax+ecx*8+0x12]",
			"mov cl,[fs:eax+ecx-0x12345678]",
			"mov cl,[fs:eax+ecx*2-0x12345678]",
			"mov cl,[fs:eax+ecx*4-0x12345678]",
			"mov cl,[fs:eax+ecx*8-0x12345678]",
			"mov cl,[fs:eax+ecx+0x12345678]",
			"mov cl,[fs:eax+ecx*2+0x12345678]",
			"mov cl,[fs:eax+ecx*4+0x12345678]",
			"mov cl,[fs:eax+ecx*8+0x12345678]",
			"mov cl,[fs:ecx*1-0x12345678]",
			"mov cl,[fs:ebp+ecx*2-0x12]",
			"mov cl,[fs:ebp+ecx*4-0x12345678]",
			"mov cl,[fs:ecx*8-0x12345678]",
			"mov cl,[fs:ecx*1+0x12345678]",
			"mov cl,[fs:ebp+ecx*2+0x12]",
			"mov cl,[fs:ebp+ecx*4+0x12345678]",
			"mov cl,[fs:ecx*8+0x12345678]",
		};
	}
}
#endif

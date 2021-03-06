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

#if !NO_MASM_FORMATTER && !NO_FORMATTER
using System.Collections.Generic;
using Xunit;

namespace Iced.UnitTests.Intel.FormatterTests.Masm {
	public sealed class MasmFormatterTest16_Misc : FormatterTest {
		[Theory]
		[MemberData(nameof(Format_Data))]
		void Format(int index, InstructionInfo info, string formattedString) => FormatBase(index, info, formattedString, MasmFormatterFactory.Create());
		public static IEnumerable<object[]> Format_Data => GetFormatData(infos, formattedStrings);

		static readonly InstructionInfo[] infos = InstructionInfos16_Misc.AllInfos;
		static readonly string[] formattedStrings = new string[InstructionInfos16_Misc.AllInfos_Length] {
			"hnt jo short 7FF3h",
			"hnt jno short 7FF3h",
			"hnt jb short 7FF3h",
			"hnt jae short 7FF3h",
			"hnt je short 7FF3h",
			"hnt jne short 7FF3h",
			"hnt jbe short 7FF3h",
			"hnt ja short 7FF3h",
			"hnt js short 7FF3h",
			"hnt jns short 7FF3h",
			"hnt jp short 7FF3h",
			"hnt jnp short 7FF3h",
			"hnt jl short 7FF3h",
			"hnt jge short 7FF3h",
			"hnt jle short 7FF3h",
			"hnt jg short 7FF3h",
			"ht jo short 7FF3h",
			"ht jno short 7FF3h",
			"ht jb short 7FF3h",
			"ht jae short 7FF3h",
			"ht je short 7FF3h",
			"ht jne short 7FF3h",
			"ht jbe short 7FF3h",
			"ht ja short 7FF3h",
			"ht js short 7FF3h",
			"ht jns short 7FF3h",
			"ht jp short 7FF3h",
			"ht jnp short 7FF3h",
			"ht jl short 7FF3h",
			"ht jge short 7FF3h",
			"ht jle short 7FF3h",
			"ht jg short 7FF3h",
			"hnt jo near ptr 7FF5h",
			"hnt jno near ptr 7FF5h",
			"hnt jb near ptr 7FF5h",
			"hnt jae near ptr 7FF5h",
			"hnt je near ptr 7FF5h",
			"hnt jne near ptr 7FF5h",
			"hnt jbe near ptr 7FF5h",
			"hnt ja near ptr 7FF5h",
			"hnt js near ptr 7FF5h",
			"hnt jns near ptr 7FF5h",
			"hnt jp near ptr 7FF5h",
			"hnt jnp near ptr 7FF5h",
			"hnt jl near ptr 7FF5h",
			"hnt jge near ptr 7FF5h",
			"hnt jle near ptr 7FF5h",
			"hnt jg near ptr 7FF5h",
			"ht jo near ptr 7FF5h",
			"ht jno near ptr 7FF5h",
			"ht jb near ptr 7FF5h",
			"ht jae near ptr 7FF5h",
			"ht je near ptr 7FF5h",
			"ht jne near ptr 7FF5h",
			"ht jbe near ptr 7FF5h",
			"ht ja near ptr 7FF5h",
			"ht js near ptr 7FF5h",
			"ht jns near ptr 7FF5h",
			"ht jp near ptr 7FF5h",
			"ht jnp near ptr 7FF5h",
			"ht jl near ptr 7FF5h",
			"ht jge near ptr 7FF5h",
			"ht jle near ptr 7FF5h",
			"ht jg near ptr 7FF5h",
			"bnd jo short 7FF3h",
			"bnd jno short 7FF3h",
			"bnd jb short 7FF3h",
			"bnd jae short 7FF3h",
			"bnd je short 7FF3h",
			"bnd jne short 7FF3h",
			"bnd jbe short 7FF3h",
			"bnd ja short 7FF3h",
			"bnd js short 7FF3h",
			"bnd jns short 7FF3h",
			"bnd jp short 7FF3h",
			"bnd jnp short 7FF3h",
			"bnd jl short 7FF3h",
			"bnd jge short 7FF3h",
			"bnd jle short 7FF3h",
			"bnd jg short 7FF3h",
			"bnd jo near ptr 7FF5h",
			"bnd jno near ptr 7FF5h",
			"bnd jb near ptr 7FF5h",
			"bnd jae near ptr 7FF5h",
			"bnd je near ptr 7FF5h",
			"bnd jne near ptr 7FF5h",
			"bnd jbe near ptr 7FF5h",
			"bnd ja near ptr 7FF5h",
			"bnd js near ptr 7FF5h",
			"bnd jns near ptr 7FF5h",
			"bnd jp near ptr 7FF5h",
			"bnd jnp near ptr 7FF5h",
			"bnd jl near ptr 7FF5h",
			"bnd jge near ptr 7FF5h",
			"bnd jle near ptr 7FF5h",
			"bnd jg near ptr 7FF5h",
			"bnd jmp near ptr 7FF4h",
			"bnd jmp word ptr [bx+si]",
			"bnd jmp ax",
			"bnd call 7FF4h",
			"bnd call word ptr [bx+si]",
			"bnd call ax",
			"bnd ret 0",
			"bnd ret",
		};
	}
}
#endif

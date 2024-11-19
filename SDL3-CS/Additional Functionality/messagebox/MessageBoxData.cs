﻿#region License
/* Copyright (c) 2024 Eduard Gushchin.
 *
 * This software is provided 'as-is', without any express or implied warranty.
 * In no event will the authors be held liable for any damages arising from
 * the use of this software.
 *
 * Permission is granted to anyone to use this software for any purpose,
 * including commercial applications, and to alter it and redistribute it
 * freely, subject to the following restrictions:
 *
 * 1. The origin of this software must not be misrepresented; you must not
 * claim that you wrote the original software. If you use this software in a
 * product, an acknowledgment in the product documentation would be
 * appreciated but is not required.
 *
 * 2. Altered source versions must be plainly marked as such, and must not be
 * misrepresented as being the original software.
 *
 * 3. This notice may not be removed or altered from any source distribution.
 */
#endregion

using System.Runtime.InteropServices;

namespace SDL3;

public static partial class SDL
{
    /// <summary>
    /// MessageBox structure containing title, text, window, etc.
    /// </summary>
    /// <param name="flags"><see cref="MessageBoxFlags"/></param>
    /// <param name="window">Parent window, can be <c>NULL</c></param>
    /// <param name="title">UTF-8 title</param>
    /// <param name="message">UTF-8 message text</param>
    /// <param name="buttons"><see cref="MessageBoxButtonData"/></param>
    /// <param name="colorScheme"><see cref="MessageBoxColorScheme"/>, can be <c>NULL</c> to use system settings</param>
    [StructLayout(LayoutKind.Sequential)]
    public struct MessageBoxData
    {
        public MessageBoxFlags Flags;
        public IntPtr Window;
        public IntPtr Title;
        public IntPtr Message;
        public int NumButtons;
        public IntPtr Buttons;
        public IntPtr ColorScheme;
    }
}
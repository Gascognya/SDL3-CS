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
using System.Runtime.CompilerServices;

namespace SDL3;

public static partial class SDL
{
    [LibraryImport(SDLLibrary), UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    private static partial IntPtr SDL_CreateRenderer(IntPtr window, [MarshalAs(UnmanagedType.LPUTF8Str)] string? name);

    public static Render? CreateRenderer(Window window, string? name)
    {
        var renderHandle = SDL_CreateRenderer(window.Handle, name);
        return renderHandle != IntPtr.Zero ? new Render(renderHandle) : null;
    }
    
    
    [LibraryImport(SDLLibrary), UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    private static partial int SDL_SetRenderVSync(IntPtr renderer, int vsync);
    public static int SetRenderVSync(Render renderer, int vsync) => SDL_SetRenderVSync(renderer.Handle, vsync);
    
    
    [LibraryImport(SDLLibrary), UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    private static partial int SDL_GetRenderVSync(IntPtr renderer, out int vsync);
    public static int GetRenderVSync(Render renderer, out int vsync) =>
        SDL_GetRenderVSync(renderer.Handle, out vsync);


    [LibraryImport(SDLLibrary), UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    private static partial void SDL_SetRenderDrawColor(IntPtr renderer, byte r, byte g, byte b, byte a);
    public static void SetRenderDrawColor(Render renderer, byte r, byte g, byte b, byte a) =>
        SDL_SetRenderDrawColor(renderer.Handle, r, g, b, a);
    
    
    [LibraryImport(SDLLibrary), UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    private static partial int SDL_RenderClear(IntPtr renderer);
    public static int RenderClear(Render renderer) => SDL_RenderClear(renderer.Handle);
    
    
    [LibraryImport(SDLLibrary), UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    private static partial int SDL_RenderPresent(IntPtr renderer);
    public static int RenderPresent(Render renderer) => SDL_RenderPresent(renderer.Handle);
    
    
    [LibraryImport(SDLLibrary), UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    private static partial void SDL_DestroyRenderer(IntPtr renderer);
    public static void DestroyRenderer(Render renderer) => SDL_DestroyRenderer(renderer.Handle);


    [LibraryImport(SDLLibrary), UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    private static partial IntPtr SDL_GetRenderDriver(int index);
    public static string? GetRenderDriver(int index) => Marshal.PtrToStringUTF8(SDL_GetRenderDriver(index));
    
    
    [LibraryImport(SDLLibrary), UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    private static partial int SDL_GetNumRenderDrivers();
    public static int GetNumRenderDrivers() => SDL_GetNumRenderDrivers();

    
    [LibraryImport(SDLLibrary), UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    private static partial IntPtr SDL_CreateTexture(IntPtr renderer, PixelFormat format, int access, int w, int h);
    public static Texture CreateTexture(Render renderer, PixelFormat format, int access, int w, int h) =>
        new(SDL_CreateTexture(renderer.Handle, format, access, w, h));
    
    
    [LibraryImport(SDLLibrary), UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    private static partial int SDL_UpdateTexture(IntPtr texture, in Rect rect, IntPtr pixels, int pitch);
    public static int UpdateTexture(Texture texture, Rect? rect, byte[] pixels, int pitch)
    {
        if (pixels == null || pixels.Length == 0)
        {
            return SDL_UpdateTexture(texture.Handle, rect.GetValueOrDefault(), IntPtr.Zero, pitch);
        }

        var handle = GCHandle.Alloc(pixels, GCHandleType.Pinned);
        try
        {
            return SDL_UpdateTexture(texture.Handle, rect.GetValueOrDefault(), handle.AddrOfPinnedObject(), pitch);
        }
        finally
        {
            handle.Free();
        }
    }
    
    
    [LibraryImport(SDLLibrary), UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    private static partial int SDL_RenderTexture(IntPtr renderer, IntPtr texture, in FRect srcrect, in FRect dstrect);
    public static int RenderTexture(Render renderer, Texture texture, FRect? srcrect, FRect? dstrect)
    {
        if (srcrect.HasValue && dstrect.HasValue)
            return SDL_RenderTexture(renderer.Handle, texture.Handle, srcrect.Value, dstrect.Value);
        if (!srcrect.HasValue && !dstrect.HasValue)
            return SDL_RenderTexture(renderer.Handle, texture.Handle, default, default);
        return srcrect.HasValue ? SDL_RenderTexture(renderer.Handle, texture.Handle, srcrect.Value, default) : 
            SDL_RenderTexture(renderer.Handle, texture.Handle, default, dstrect.Value);
    }
}
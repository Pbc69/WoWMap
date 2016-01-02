﻿using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WoWMapRenderer.Renderers;

namespace WoWMapRenderer
{
    public class TextureCache
    {
        public static Dictionary<string /* fileName */, Texture> RawTextures { get; private set; }

        public static void Initialize()
        {
            RawTextures = new Dictionary<string, Texture>(10);
        }

        public static void RemoveRawTexture(string textureName)
        {
            if (RawTextures.ContainsKey(textureName))
                RawTextures.Remove(textureName);
        }

        public static bool ContainsRawTexture(string textureName)
        {
            return RawTextures.ContainsKey(textureName);
        }

        public static Texture GetRawTexture(string textureName)
        {
            if (ContainsRawTexture(textureName))
                return RawTextures[textureName];

            var texture = new Texture(textureName);
            texture.InternalFormat = PixelInternalFormat.Rgba8;
            texture.Format = PixelFormat.Rgba;
            texture.WrapS = (int)All.Repeat;
            texture.WrapT = (int)All.Repeat;

            RawTextures[textureName] = texture;
            return texture;
        }
    }
}
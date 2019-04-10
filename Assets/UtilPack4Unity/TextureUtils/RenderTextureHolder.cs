using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace UtilPack4Unity
{
    public class RenderTextureHolder : TextureHolderBase
    {
        [SerializeField]
        protected RenderTexture renderTexture;

        public virtual RenderTexture GetRenderTexture()
        {
            return renderTexture;
        }

        public override Texture GetTexture()
        {
            return renderTexture;
        }
    }
}
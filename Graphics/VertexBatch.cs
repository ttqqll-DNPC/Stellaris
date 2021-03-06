﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Stellaris.Graphics
{
    public class VertexBatch : IDisposable
    {
        public GraphicsDevice graphicsDevice;
        public PrimitiveType primitiveType;
        private bool _begin;
        List<Vertex> vertexData;
        List<short> indexData;
        BasicEffect basicEffect;
        public VertexBatch(GraphicsDevice graphicsDevice)
        {
            this.graphicsDevice = graphicsDevice;
            basicEffect = new BasicEffect(graphicsDevice);
            basicEffect.VertexColorEnabled = true;
            basicEffect.World = Matrix.Identity;
            vertexData = new List<Vertex>();
            indexData = new List<short>();
        }
        private void OnBeginning(PrimitiveType primitiveType, BlendState blendState)
        {
            if (_begin) throw new Exception("Called Begin Twice");
            this.primitiveType = primitiveType;
            graphicsDevice.BlendState = blendState;
            RasterizerState rasterizerState = new RasterizerState();
            rasterizerState.CullMode = CullMode.None;
            graphicsDevice.RasterizerState = rasterizerState;
            basicEffect.View = Matrix.CreateTranslation(-Common.Resolution.X / 2, -Common.Resolution.Y / 2, 0) * Matrix.CreateRotationX(3.141592f);
            basicEffect.Projection = Matrix.CreateOrthographic(Common.Resolution.X, Common.Resolution.Y, -100, 100);
            _begin = true;
        }
        public void Begin(PrimitiveType primitiveType = PrimitiveType.TriangleList)
        {
            Begin(BlendState.AlphaBlend, primitiveType);
        }
        public void Begin(BlendState blendState, PrimitiveType primitiveType = PrimitiveType.TriangleList)
        {
            OnBeginning(primitiveType, blendState);
            basicEffect.TextureEnabled = false;
        }
        public void Begin(Texture2D texture2D, PrimitiveType primitiveType = PrimitiveType.TriangleList)
        {
            Begin(texture2D, BlendState.AlphaBlend, primitiveType);
        }
        public void Begin(Texture2D texture2D, BlendState blendState, PrimitiveType primitiveType = PrimitiveType.TriangleList)
        {
            OnBeginning(primitiveType, blendState);
            basicEffect.TextureEnabled = true;
            basicEffect.Texture = texture2D;
        }
        public void Draw(params Vertex[] vertex)
        {
            if (!_begin) throw new Exception("Called Draw Before Begin");
            Draw(vertex, Helper.FromAToB((short)(vertexData.Count), (short)(vertexData.Count + vertex.Length)));
        }
        public void Draw(Vertex[] vertex, params short[] index)
        {
            if (!_begin) throw new Exception("Called Draw Before Begin");
            if (vertexData.Count != 0) index.PlusAll((short)vertexData.Count);
            vertexData.AddRange(vertex);
            indexData.AddRange(index);
            if (vertexData.Count > short.MaxValue) throw new Exception("Vertices Counts Over 32768");
        }
        public void Draw(VertexInfo vertexInfo)
        {
            Draw(vertexInfo.vertex, vertexInfo.index);
        }
        public static int LengthGusser(int length, PrimitiveType primitiveType)
        {
            return primitiveType == PrimitiveType.TriangleList ? length / 3 : (primitiveType == PrimitiveType.LineList ? length / 2 : (primitiveType == PrimitiveType.TriangleStrip ? length - 2 : length - 1));
        }
        public void DoDraw()
        {
            if (!_begin) throw new Exception("Called Draw Before Begin");
            if (vertexData.Count == 0) return;
            Vertex[] array = vertexData.ToArray();
            int length = indexData.Count == 0 ? vertexData.Count : indexData.Count;
            length = LengthGusser(length, primitiveType);
            basicEffect.CurrentTechnique.Passes[0].Apply();
            graphicsDevice.DrawUserIndexedPrimitives(primitiveType, array, 0, array.Length, indexData.ToArray(), 0, length);
            vertexData.Clear();
            indexData.Clear();
        }
        public void End()
        {
            DoDraw();
            _begin = false;
        }
        public void Dispose()
        {
            vertexData.Clear();
            indexData.Clear();
            basicEffect = null;
        }
    }
}

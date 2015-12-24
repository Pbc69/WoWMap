﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace WoWMapRenderer
{
    class Shader
    {
        private int VertexID;
        private int FragmentID;
        public int ProgramID;

        private Dictionary<string, int> _attribLocations = new Dictionary<string, int>();
        private Dictionary<string, int> _uniformLocations = new Dictionary<string, int>(); 

        public void CreateShader(string vertex, string fragment)
        {
            ProgramID = GL.CreateProgram();

            VertexID = GL.CreateShader(ShaderType.VertexShader);
            GL.ShaderSource(VertexID, vertex);
            GL.CompileShader(VertexID);
            GL.AttachShader(ProgramID, VertexID);
            var vertexErr = GL.GetShaderInfoLog(VertexID);

            FragmentID = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(FragmentID, fragment);
            GL.CompileShader(FragmentID);
            GL.AttachShader(ProgramID, FragmentID);
            var fragmentErr = GL.GetShaderInfoLog(FragmentID);
            
            GL.LinkProgram(ProgramID);
        }

        public void SetCurrent()
        {
            GL.UseProgram(ProgramID);
        }

        public int GetAttribLocation(string attribName)
        {
            if (!_attribLocations.ContainsKey(attribName))
                _attribLocations[attribName] = GL.GetAttribLocation(ProgramID, attribName);
            Debug.Assert(_attribLocations[attribName] != -1, "glGetAttribLocation(" + attribName + ")");
            return _attribLocations[attribName];
        }

        public int GetUniformLocation(string attrName)
        {
            if (!_uniformLocations.ContainsKey(attrName))
                _uniformLocations[attrName] = GL.GetUniformLocation(ProgramID, attrName);
            Debug.Assert(_uniformLocations[attrName] != -1, "glGetUniformLocation(" + attrName + ")");
            return _uniformLocations[attrName];
        }
    }
}
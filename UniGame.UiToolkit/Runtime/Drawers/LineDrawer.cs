namespace UniGame.UiToolkit.Runtime.Drawers
{
    using UnityEngine;
    using UnityEngine.UIElements;

    using System;

    [Serializable]
    public class LineDrawer : VisualElement
    {
        public Vector3 startPos;
        public Vector3 endPos;
        public float thickness;

        public LineDrawer(Vector3 pos1, Vector3 pos2, float width)
        {
            startPos = pos1;
            endPos = pos2;
            thickness = width;

            generateVisualContent += OnGenerateVisualContent;
        }

        private void OnGenerateVisualContent(MeshGenerationContext ctx)
        {
            var angleDeg = Vector3.Angle(startPos, endPos);

            MeshWriteData mesh = ctx.Allocate(4, 6);
            Vertex[] vertices = new Vertex[4];
            vertices[0].position = startPos - new Vector3(0, thickness / 2, 0); //bottom left
            vertices[1].position = startPos + new Vector3(0, thickness / 2, 0); //top left
            vertices[2].position = endPos + new Vector3(0, thickness / 2, 0); //top right
            vertices[3].position = endPos - new Vector3(0, thickness / 2, 0); //bottom right

            for (var index = 0; index < vertices.Length; index++)
            {
                vertices[index].position += Vector3.forward * Vertex.nearZ;
                vertices[index].tint = Color.white;
            }

            mesh.SetAllVertices(vertices);
            mesh.SetAllIndices(new ushort[] { 0, 1, 3, 1, 2, 3 });
        }
    }
}
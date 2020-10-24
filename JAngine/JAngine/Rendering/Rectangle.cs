// using OpenTK.Graphics.OpenGL4;
// using OpenTK.Mathematics;
//
// namespace JAngine.Rendering
// {
//     /*
//      * Border-radius [top, bottom, left, right] - floatx4
//      * Border-width [width] - floatx1
//      * Border-color [color] - floatx4
//      *
//      * Shadow - offset
//      * Shadow - color
//      * Shadow - spread
//      *
//      * Texture - image
//      * Texture - repeat
//      * Texture - area
//      * 
//      * Color [color] - floatx4
//      * Origin [normalized vec2] - floatx2
//      * Coords [pixel space vec2] - floatx2
//      */
//     public sealed class Rectangle : IDrawable
//     {
//         private static VertexArray _vao;
//         private static VertexBuffer<float> _posVbo, _uvVbo;
//         private static ElementBuffer _ebo;
//
//         static Rectangle()
//         {
//             _ebo = new ElementBuffer(new uint[]{0, 1, 2, 0, 2, 3});
//             _vao = new VertexArray(_ebo);
//             _posVbo = new VertexBuffer<float>(
//                 -0.5f,  -0.5f, 0,
//                 0.5f, -0.5f, 0,
//                 0.5f,  0.5f, 0,
//                 -0.5f,  0.5f, 0);
//             _uvVbo = new VertexBuffer<float>(
//                 0, 0,
//                 1, 0,
//                 1, 1,
//                 0, 1);
//             _vao.AddAttribute(_posVbo, 3, 0, 0);
//             _vao.AddAttribute(_uvVbo, 2, 1, 0);
//         }
//         
//         public void Draw()
//         {
//             _vao.Draw(_ebo.Count, 0, 1);
//         }
//     }
// }
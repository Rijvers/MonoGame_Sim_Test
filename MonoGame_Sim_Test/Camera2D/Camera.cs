using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGame_Sim_Test
{
    public class Camera
    {
        private Matrix transform;
        public Matrix Transform
        {
            get { return transform; }
        }

        private Vector2 center;
        private Viewport viewport;

        private float zoom = 1;
        private float rotation = 0;

        public Vector2 Get_Center()
        {
            return center;
        }

        public Vector2 Get_TopLeft()
        {
            return new Vector2(center.X, center.Y);
        }

        public float X
        {
            get { return center.X; }
            set { center.X = value; }
        }

        public float Y
        {
            get { return center.Y; }
            set { center.Y = value; }
        }

        public float Zoom
        {
            get { return zoom; }
            set
            {
                zoom = value;
                if (zoom < 0.1f)
                    zoom = 0.1f;
            }
        }

        public float Rotation
        {
            get { return rotation; }
            set { rotation = value; }
        }

        public Camera(Viewport newviewport)
        {
            viewport = newviewport;
        }

        public void Teleport(Vector2 position)
        {
            center = new Vector2(position.X, position.Y);
            transform = Matrix.CreateTranslation(new Vector3(-center.X, -center.Y, 0)) *
                Matrix.CreateRotationZ(rotation) *
                Matrix.CreateScale(new Vector3(zoom, zoom, 0)) *
                Matrix.CreateTranslation(new Vector3(viewport.Width / 2, viewport.Height / 2, 0));
        }

        public void Move(Vector2 position)
        {
            center += position;
            transform = Matrix.CreateTranslation(new Vector3(-center.X, -center.Y, 0)) *
                Matrix.CreateRotationZ(rotation) *
                Matrix.CreateScale(new Vector3(zoom, zoom, 0)) *
                Matrix.CreateTranslation(new Vector3(viewport.Width / 2, viewport.Height / 2, 0));
        }

        public Point? Get_World_Coords(Point? Screen_Coords)
        {
            return Screen_Coords + Get_TopLeft().ToPoint() - new Point(viewport.Width / 2, viewport.Height / 2);
        }

        public Point? Get_Screen_Coords(Point? World_Coords)
        {
            return World_Coords - Get_TopLeft().ToPoint() + new Point(viewport.Width / 2, viewport.Height / 2);
        }

        public Rectangle Get_Culling_Rectangle()
        {
            return new Rectangle((int)(center.X - viewport.Width / 2), (int)(center.Y - viewport.Height / 2), viewport.Width, viewport.Height);
        }

        public override string ToString()
        {
            return "Center X: " + center.X + " Y: " + center.Y + "\r\nZoom: " + zoom + "\r\nrotation: " + rotation +
                "\r\nviewport width: " + viewport.Width + " height: " + viewport.Height +
                "\r\n X: " + viewport.X + " Y: " + viewport.Y + "\r\n" + PrintViewPort();
        }

        public string PrintViewPort()
        {
            return "Viewport Top-Left: " + new Vector2(center.X - viewport.Width / 2, center.Y - viewport.Height / 2) +
                "\r\nTop-Right: " + new Vector2(center.X + viewport.Width / 2, center.Y - viewport.Height / 2) +
                "\r\nBottum-Left: " + new Vector2(center.X - viewport.Width / 2, center.Y + viewport.Height / 2) +
                "\r\nBottum-Right: " + new Vector2(center.X + viewport.Width / 2, center.Y + viewport.Height / 2);
        }
    }
}

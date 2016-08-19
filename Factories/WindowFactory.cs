using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Quarter4Project.Factories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Quarter4Project
{
    class WindowFactory
    {

        public class Okay
        {

            public List<ButtonFactory.Default> buttonList { get; private set; }
            public Vector2 pos { get; private set; }
            public Point size { get; private set; }
            public Texture2D tex { get; private set; }
            public Color color { get; private set; }
            public SpriteFont font { get; private set; }
            public string text { get; private set; }
            public Color textColor { get; private set; }
            public Vector2 textShadowPos { get; private set; }
            public Color textShadowColor { get; private set; }
            public Vector2 borderSize { get; private set; }
            public Texture2D borderTex { get; private set; }
            public Color borderColor { get; private set; }
            public bool vis { get; private set; }
            public int windowNum { get; private set; }
            public int order { get; private set; }

            public Okay(Vector2 position, Point size, Color color, GraphicsDevice graphicsDevice, SpriteFont fontFamily, string text, Color textColor, Vector2 textShadowPos, Color textShadowColor, Vector2 borderSize, Color borderColor, bool visible, int windowNum)
            {
                pos = position;
                this.size = size;
                tex = convertColorToTexture2D(color, graphicsDevice);
                this.color = color;
                font = fontFamily;
                this.text = text;
                this.textColor = textColor;
                this.textShadowPos = textShadowPos;
                this.textShadowColor = textShadowColor;
                this.borderSize = borderSize;
                borderTex = convertColorToTexture2D(borderColor, graphicsDevice);
                this.borderColor = borderColor;
                vis = visible;
                this.windowNum = windowNum;
            }

            public Okay(Vector2 position, Point size, Texture2D bgTex, SpriteFont fontFamily, string text, Color textColor, Vector2 textShadowPos, Color textShadowColor, bool visible, int windowNum)
            {
                pos = position;
                this.size = size;
                tex = bgTex;
                font = fontFamily;
                this.text = text;
                this.textColor = textColor;
                this.textShadowPos = textShadowPos;
                this.textShadowColor = textShadowColor;
                vis = visible;
                this.windowNum = windowNum;
                this.color = Color.Transparent;
            }

            /// <summary>
            /// Converts a color into a Texture2D.
            /// </summary>
            /// <param name="c">Color.</param>
            /// <param name="gD">Graphics Device for conversion.</param>
            /// <returns>Texture2D.</returns>
            private Texture2D convertColorToTexture2D(Color c, GraphicsDevice gD)
            {
                Texture2D texture2d = new Texture2D(gD, 1, 1, false, SurfaceFormat.Color);
                texture2d.SetData<Color>(new Color[] { new Color(c.R, c.G, c.B, c.A) });

                return texture2d;
            }

            /// <summary>
            /// Returns collision rectangle
            /// </summary>
            /// <returns>Rectangle</returns>
            public Rectangle collisionRect()
            {
                return new Rectangle((int)pos.X, (int)pos.Y, size.X, size.Y);
            }

            /// <summary>
            /// Sets the windows buttons.
            /// </summary>
            /// <param name="buttonList">List of buttons.</param>
            public void setButtons(List<ButtonFactory.Default> btnList)
            {
                this.buttonList = new List<ButtonFactory.Default>();
                foreach (ButtonFactory.Default b in btnList)
                    buttonList.Add(b);
            }

            /// <summary>
            /// Sets new values in the window.
            /// </summary>
            /// <param name="position"></param>
            /// <param name="size"></param>
            /// <param name="color"></param>
            /// <param name="graphicsDevice"></param>
            /// <param name="fontFamily"></param>
            /// <param name="text"></param>
            /// <param name="textColor"></param>
            /// <param name="textShadowPos"></param>
            /// <param name="textShadowColor"></param>
            /// <param name="borderSize"></param>
            /// <param name="borderColor"></param>
            /// <param name="visible"></param>
            public void setNewValues(Vector2 position, Point size, Color color, GraphicsDevice graphicsDevice, SpriteFont fontFamily, string text, Color textColor, Vector2 textShadowPos, Color textShadowColor, Vector2 borderSize, Color borderColor, bool visible)
            {
                pos = position;
                this.size = size;
                tex = convertColorToTexture2D(color, graphicsDevice);
                this.color = color;
                font = fontFamily;
                this.text = text;
                this.textColor = textColor;
                this.textShadowPos = textShadowPos;
                this.textShadowColor = textShadowColor;
                this.borderSize = borderSize;
                borderTex = convertColorToTexture2D(borderColor, graphicsDevice);
                this.borderColor = borderColor;
                vis = visible;
            }

            public void setNewValues(Vector2 position, Point Size, Texture2D tex, SpriteFont fontFamily, string text, Color textColor, Vector2 textShadowPos, Color textShadowColor, bool visible)
            {
                pos = position;
                this.size = size;
                this.tex = tex;
                this.color = color;
                font = fontFamily;
                this.text = text;
                this.textColor = textColor;
                this.textShadowPos = textShadowPos;
                this.textShadowColor = textShadowColor;
                vis = visible;
            }

            public void setVisibility(bool vis)
            {
                this.vis = vis;
            }

            public void setPos(Vector2 pos)
            {
                this.pos = pos;
            }

            public void setOrder(int o)
            {
                order = o;
            }

        }

    }
}

// ButtonFactory.cs

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Quarter4Project.Libraries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Quarter4Project.Factories
{
    class ButtonFactory
    {

        /// <summary>
        /// Default button creation.
        /// </summary>
        public class Default
        {

            #region Fields

            public Vector2 pos { get; private set; }
            public Point size { get; private set; }
            public Texture2D tex { get; private set; }
            public SpriteFont fontFamily { get; private set; }
            public string text { get; private set; }
            public Color textColor { get; private set; }
            public Vector2 textShadowPos { get; private set; }
            public Color textShadowColor { get; private set; }
            public Vector2 borderSize { get; private set; }
            public Texture2D borderTex { get; private set; }
            public int hoverNum { get; private set; }
            public int eventNum { get; private set; }
            public int windowNum { get; private set; }
            public Vector2 origPos { get; private set; }

            #endregion

            #region Initialization

            /// <summary>
            /// 
            /// </summary>
            /// <param name="buttonPosition"></param>
            /// <param name="buttonSize"></param>
            /// <param name="buttonColor"></param>
            /// <param name="graphicsDevice"></param>
            /// <param name="eventID"></param>
            public Default(Vector2 buttonPosition, Point buttonSize, Color buttonColor, GraphicsDevice graphicsDevice, SpriteFont fontFamily, string buttonText, Color textColor, Vector2 textShadowPos, Color textShadowColor, Vector2 borderSize, Color borderColor, int hoverID, int eventID, int windowID)
            {
                pos = buttonPosition;
                size = buttonSize;
                tex = convertColorToTexture2D(buttonColor, graphicsDevice);
                this.fontFamily = fontFamily;
                text = buttonText;
                this.textColor = textColor;
                this.textShadowPos = textShadowPos;
                this.textShadowColor = textShadowColor;
                this.borderSize = borderSize;
                borderTex = convertColorToTexture2D(borderColor, graphicsDevice);
                hoverNum = hoverID;
                eventNum = eventID;
                windowNum = windowID;
                origPos = buttonPosition;
            }

            #endregion

            #region Methods

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
            /// Gets the center of the button.
            /// </summary>
            /// <returns>The vector2 center of the button.</returns>
            public Vector2 getButtonCenter()
            {
                return new Vector2(pos.X + (size.X / 2), pos.Y + (size.Y / 2));
            }

            /// <summary>
            /// Gets collision rectangle of button.
            /// </summary>
            /// <returns>Rectangle.</returns>
            public Rectangle collisionRectangle()
            {
                return new Rectangle((int)pos.X, (int)pos.Y, size.X, size.Y);
            }

            public void setPos(Vector2 pos)
            {
                this.pos = pos;
            }

            #endregion

        }

        /// <summary>
        /// Button creation w/ texture animation.
        /// </summary>
        public class Texture : Animation
        {

            #region Fields

            public Vector2 pos { get; private set; }
            public Point size { get; private set; }
            public Texture2D tex { get; private set; }
            public int hoverNum { get; private set; }
            public int eventNum { get; private set; }
            public int windowNum { get; private set; }
            public Vector2 origPos { get; private set; }
            private List<TextureAnimation> animList;
            public int type { get; private set; }

            #endregion

            #region Initialization

            public Texture(List<TextureAnimation> texAnims, Vector2 buttonPosition, int hoverID, int eventID, int windowID)
                : base(buttonPosition)
            {
                animList = texAnims;
                pos = buttonPosition;
                hoverNum = hoverID;
                eventNum = eventID;
                windowNum = windowID;
                origPos = pos;
                addAnimations();
                type = 1;
            }

            #endregion

            #region Methods

            /// <summary>
            /// Adds all the animations from the animation list.
            /// </summary>
            public void addAnimations()
            {
                for (int i = 0; i < animList.Count; i++)
                {
                    addAnimation(animList[i].name, animList[i].texture, animList[i].frameSize, animList[i].sheetSize, animList[i].startPos, animList[i].endPos, animList[i].milliPerFrame, animList[i].wash);
                    setAnimation(animList[0].name);
                }
            }

            /// <summary>
            /// Gets the center of the button.
            /// </summary>
            /// <returns>The vector2 center of the button.</returns>
            public Vector2 getButtonCenter()
            {
                return new Vector2(pos.X + (size.X / 2), pos.Y + (size.Y / 2));
            }

            /// <summary>
            /// Gets collision rectangle of button.
            /// </summary>
            /// <returns>Rectangle.</returns>
            public Rectangle collisionRectangle()
            {
                return new Rectangle((int)pos.X, (int)pos.Y, size.X, size.Y);
            }
            
            public new void setPos(Vector2 pos)
            {
                this.pos = pos;
            }

            #endregion

            #region Sub-Classes

            public class TextureAnimation
            {

                #region Fields

                public string name { get; private set; }
                public Texture2D texture { get; private set; }
                public Point frameSize { get; private set; }
                public Point sheetSize { get; private set; }
                public Point startPos { get; private set; }
                public Point endPos { get; private set; }
                public int milliPerFrame { get; private set; }
                public Color wash { get; private set; }

                #endregion

                #region Initialization

                /// <summary>
                /// Adds an animation based on the following parameters.
                /// </summary>
                /// <param name="name">Name of the animation</param>
                /// <param name="tex">Sprite sheet used in the animation</param>
                /// <param name="frameSize">Frame size of the animation in pixels.</param>
                /// <param name="sheetSize">Sheet size of the total frames used</param>
                /// <param name="startPos">Starting position of the animation.</param>
                /// <param name="endPos">Ending position of the animation.</param>
                /// <param name="milliPerFrame">Frame change rate.</param>
                /// <param name="wash">Color wash.</param>
                public TextureAnimation(string name, Texture2D tex, Point frameSize, Point sheetSize, Point startPos, Point endPos, int milliPerFrame, Color wash)
                {
                    this.name = name;
                    texture = tex;
                    this.frameSize = frameSize;
                    this.sheetSize = sheetSize;
                    this.startPos = startPos;
                    this.endPos = endPos;
                    this.milliPerFrame = milliPerFrame;
                    this.wash = wash;
                }

                #endregion

            }

            #endregion

        }

    }
}

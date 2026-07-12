using Microsoft.Xna.Framework;
using NuciXNA.Graphics.Drawing;
using NuciXNA.Gui.Controls;
using NuciXNA.Primitives;

namespace DoomRPG.Gui.GuiElements
{
    public sealed class GuiDialogue : GuiControl
    {
        const int Padding = 12;

        GuiImage background;
        GuiText text;

        public void Show(string dialogue)
        {
            text.Text = dialogue.Replace("|", "\n");
            IsVisible = true;
        }

        public new void Hide()
        {
            text.Text = string.Empty;
            IsVisible = false;
        }

        protected override void DoLoadContent()
        {
            background = new GuiImage
            {
                ContentFile = "ScreenManager/FillImage",
                TintColour = Colour.Black
            };

            text = new GuiText
            {
                FontName = "MenuFont",
                ForegroundColour = Colour.White,
                HorizontalAlignment = Alignment.Middle
            };

            RegisterChildren(background, text);

            IsVisible = false;
        }

        protected override void DoUnloadContent() { }

        protected override void DoUpdate(GameTime gameTime)
        {
            background.Location = Point2D.Empty;
            background.Size = Size;

            text.Location = new Point2D(Padding, Padding);
            text.Size = new Size2D(Size.Width - Padding * 2, Size.Height - Padding * 2);
        }

        protected override void DoDraw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch) { }
    }
}

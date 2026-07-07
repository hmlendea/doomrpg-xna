using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NuciXNA.Input;
using NuciXNA.Primitives;

using NuciXNA.Gui;
using NuciXNA.Gui.Controls;
using NuciXNA.Gui.Screens;

namespace DoomRPG.Gui.Screens
{
    /// <summary>
    /// Splash screen.
    /// </summary>
    public class SplashScreen : Screen
    {
        /// <summary>
        /// Gets or sets the delay.
        /// </summary>
        /// <value>The delay.</value>
        public float Delay { get; set; }
        
        GuiImage logoImage { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SplashScreen"/> class.
        /// </summary>
        public SplashScreen()
        {
            Delay = 3;
            BackgroundColour = Colour.Black;
        }

        /// <summary>
        /// Loads the content.
        /// </summary>
        protected override void DoLoadContent()
        {
            logoImage = new GuiImage { ContentFile = "SplashScreen/Logo" };
            
            GuiManager.Instance.RegisterControls(logoImage);

            RegisterEvents();
            SetChildrenProperties();
        }

        /// <summary>
        /// Unloads the content.
        /// </summary>
        protected override void DoUnloadContent()
        {
            UnregisterEvents();
        }

        /// <summary>
        /// Updates the content.
        /// </summary>
        /// <param name="gameTime">Game time.</param>
        protected override void DoUpdate(GameTime gameTime)
        {
            SetChildrenProperties();
            
            Delay -= (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        protected override void DoDraw(SpriteBatch spriteBatch)
        {

        }

        /// <summary>
        /// Registers the events.
        /// </summary>
        void RegisterEvents()
        {
            KeyPressed += OnKeyPressed;
            MouseButtonPressed += OnMouseButtonPressed;
        }

        /// <summary>
        /// Unregisters the events.
        /// </summary>
        void UnregisterEvents()
        {
            KeyPressed -= OnKeyPressed;
            MouseButtonPressed -= OnMouseButtonPressed;
        }

        void SetChildrenProperties()
        {
            logoImage.Location = new Point2D((ScreenManager.Instance.Size.Width - logoImage.Size.Width) / 2,
                                             (ScreenManager.Instance.Size.Height - logoImage.Size.Height) / 2);
        }

        void OnKeyPressed(object sender, KeyboardKeyEventArgs e)
        {
            ScreenManager.Instance.ChangeScreens(typeof(GameplayScreen));
        }

        void OnMouseButtonPressed(object sender, MouseButtonEventArgs e)
        {
            ScreenManager.Instance.ChangeScreens(typeof(GameplayScreen));
        }
    }
}

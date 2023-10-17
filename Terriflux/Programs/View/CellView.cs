using Godot;
using System;
using System.ComponentModel;
using System.IO;
using System.Text;
using Terriflux.Programs.GameContext;
using Terriflux.Programs.Observers;

namespace Terriflux.Programs.View
{
    public partial class CellView : Node2D, IVerbosable     // Reworked
    {
        protected static readonly string defaultTexture = OurPaths.TEXTURES + "default" + OurPaths.PNGEXT;

        // children
        private Label _nicknameLabel;     
        public Sprite2D _skin;

        // Creation
        /// <summary>
        /// Create a view for any kind of cell.
        /// Careful: Simple class construction not allowed. Please use the associated Design() function!
        /// </summary>
        protected CellView() { }

        public override void _Ready()
        {
            base._Ready();
            _nicknameLabel = GetNode<Label>("NicknameLabel");
            _skin = GetNode<Sprite2D>("Skin");

            // Hide useless
            _nicknameLabel.Hide();

            // default
            ChangeName("Cell");
            ChangeSkin(GD.Load<Texture2D>(defaultTexture));
        }

        /// <summary>
        /// Design a CellView. 
        /// Remember to add it to your scene to display it!
        /// </summary>
        /// <returns></returns>
        public static CellView Design()
        {
            return (CellView)GD.Load<PackedScene>(OurPaths.VIEW_NODES + "CellView" + OurPaths.GDEXT)
                .Instantiate();
        }

        // Skin
        /// <summary>
        /// Changes the image representing this cell on the screen
        /// </summary>
        /// <param name="skin"></param>
        /// <exception cref="ArgumentNullException"></exception>
        protected void ChangeSkin(Texture2D skin)
        {
            if (_skin == null)
            {
                throw new NullReferenceException(this + "'s skin child not loaded correctly!");
            }
            else if (skin == null)
            {
                throw new ArgumentNullException(nameof(skin));
            }
            else
            {
                _skin.Texture = skin;
            }
        }

        protected void ChangeName(string name)
        {
            if (_nicknameLabel != null)
            {
                _nicknameLabel.Text = name;
            }
        }

        // Verbose
        public string Verbose()
        {
            StringBuilder sb = new();
            sb.Append("Cell " + this);
            if (_skin == null)
            {
                sb.Append("_skin null");
            }
            else
            {
                sb.Append("Skin = " + _skin.Texture.ResourceName);
            }
            if (_nicknameLabel == null)
            {
                sb.Append("_nicknameLabel null");
            }
            return sb.ToString();
        }

        // Events
        private void OnMouseOver()
        {
            _nicknameLabel.Show();
        }

        private void OnMouseExit()
        {
            _nicknameLabel.Hide();
        }
    }
}
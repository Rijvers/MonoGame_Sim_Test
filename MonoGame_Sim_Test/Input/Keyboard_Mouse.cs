using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace MonoGame_Sim_Test
{
    public enum Numerics
    {
        Camera_Zoom,
        Camera_Rotation
    }

    public enum Bools
    {
        Debug_Info
    }

    public enum Input_Type
    {
        Click,
        Release,
        Hold,
        Toggle,
        Always
    }

    internal class Keyboard_Mouse
    {
        private Keyboard_Mouse_Input keyboard_Mouse_Input;
        private Four_Directional_Controls four_directional_Controls_Arrows, four_directional_Controls_WASD;
        private readonly Dictionary<Numerics, Numeric_Controls> Dic_Numeric_Controlers = new Dictionary<Numerics, Numeric_Controls>();
        private readonly Dictionary<Bools, Bool_Controls> Dic_Bool_Controllers = new Dictionary<Bools, Bool_Controls>();
        private ScrollWheel_Controls ScrollWheel_Controls;
        private readonly Mouse_Left_Button mouse_Left_Button = new Mouse_Left_Button();

        public Keyboard_Mouse(Game1 game)
        {
            Init(game);
        }

        public void Init(Game1 game)
        {
            keyboard_Mouse_Input = new Keyboard_Mouse_Input(game);

            four_directional_Controls_Arrows = new Four_Directional_Controls(
                new List<Keys> { Keys.Right, Keys.Left, Keys.Up, Keys.Down }, 0.25f);
            four_directional_Controls_WASD = new Four_Directional_Controls(
                new List<Keys> { Keys.D, Keys.A, Keys.W, Keys.S }, 0.25f);

            Dic_Numeric_Controlers.Add(Numerics.Camera_Zoom,
                new Numeric_Controls(new List<Keys> { Keys.OemPlus, Keys.OemMinus }, 1f, 0.001f));
            Dic_Numeric_Controlers.Add(Numerics.Camera_Rotation,
                new Numeric_Controls(new List<Keys> { Keys.OemOpenBrackets, Keys.OemCloseBrackets }, 0, 0.0025f));

            Dic_Bool_Controllers.Add(Bools.Debug_Info, new Bool_Controls(Keys.F1));

            ScrollWheel_Controls = new ScrollWheel_Controls(1 / 600f);
        }

        public void Update(GameTime gameTime) //call ONCE every update, THEN run the input checks
        {
            keyboard_Mouse_Input.Update(gameTime);
        }

        public Vector2 Get_Main_Movement()
        {
            Vector2 movement = four_directional_Controls_WASD.Get_Update(keyboard_Mouse_Input);
            movement += four_directional_Controls_Arrows.Get_Update(keyboard_Mouse_Input);

            return movement;
        }

        public float Get_Camera_Zoom(float Current_Zoom)
        {
            return Dic_Numeric_Controlers[Numerics.Camera_Zoom].Get_Value(keyboard_Mouse_Input, Current_Zoom);
        }

        public float Get_Camera_Rotation(float Current_Rotation)
        {
            return Dic_Numeric_Controlers[Numerics.Camera_Rotation].Get_Value(keyboard_Mouse_Input, Current_Rotation);
        }

        public bool Get_Debug_State(bool ShowDebug)
        {
            return Dic_Bool_Controllers[Bools.Debug_Info].Get_Update(keyboard_Mouse_Input, ShowDebug);
        }

        public float Get_ScrollWheel()
        {
            return ScrollWheel_Controls.Get_Update(keyboard_Mouse_Input);
        }

        public Point? Get_Mouse_LeftButton_Down()
        {
            mouse_Left_Button.Change_Input_Type(Input_Type.Hold);
            return mouse_Left_Button.Get_Update(keyboard_Mouse_Input);
        }

        public Point? Get_Mouse_Position()
        {
            mouse_Left_Button.Change_Input_Type(Input_Type.Always);
            return mouse_Left_Button.Get_Update(keyboard_Mouse_Input);
        }

        public Point? Get_Mouse_LeftButton_Click()
        {
            mouse_Left_Button.Change_Input_Type(Input_Type.Click);
            return mouse_Left_Button.Get_Update(keyboard_Mouse_Input);
        }

        public Point? Get_Mouse_LeftButton_Release()
        {
            mouse_Left_Button.Change_Input_Type(Input_Type.Release);
            return mouse_Left_Button.Get_Update(keyboard_Mouse_Input);
        }
    }

    internal class Keyboard_Mouse_Input
    {
        private readonly Game1 game;

        internal KeyboardState KeyState = new KeyboardState();
        internal KeyboardState PreviousKeyState = new KeyboardState();

        internal MouseState MouseState = new MouseState();
        internal MouseState PreviousMouseState = new MouseState();

        internal float timepassed = 0;

        internal Keyboard_Mouse_Input(Game1 game)
        {
            this.game = game;
        }

        internal void Update(GameTime gameTime)
        {
            PreviousKeyState = KeyState;
            PreviousMouseState = MouseState;
            KeyState = Keyboard.GetState();
            MouseState = Mouse.GetState();

            timepassed = (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            if (KeyState.IsKeyDown(Keys.Escape))
                game.Quit();
        }
    }

    internal class Four_Directional_Controls
    {
        private List<Keys> Direction_keys = new List<Keys>(); //right, left, up, down
        private float increase_Value = 1f;

        internal Four_Directional_Controls(List<Keys> Main_Direction_keys, float increase_Value)
        {
            Change_Keys(Main_Direction_keys);
            Change_increase_Value(increase_Value);
        }

        internal void Change_Keys(List<Keys> Main_Direction_keys)
        {
            this.Direction_keys = Main_Direction_keys;
        }

        internal void Change_increase_Value(float increase_Value)
        {
            this.increase_Value = increase_Value;
        }

        internal Vector2 Get_Update(Keyboard_Mouse_Input KM_input)
        {
            Vector2 MovementUpdate = Vector2.Zero;
            if (KM_input.KeyState.IsKeyDown(Direction_keys[0]))
            {//right
                MovementUpdate.X += 1f;
            }
            if (KM_input.KeyState.IsKeyDown(Direction_keys[1]))
            {//left
                MovementUpdate.X -= 1f;
            }
            if (KM_input.KeyState.IsKeyDown(Direction_keys[2]))
            {//up
                MovementUpdate.Y -= 1f;
            }
            if (KM_input.KeyState.IsKeyDown(Direction_keys[3]))
            {//down
                MovementUpdate.Y += 1f;
            }
            if (MovementUpdate.X != 0 || MovementUpdate.Y != 0)
            {
                MovementUpdate.Normalize();
                MovementUpdate *= increase_Value;
            }
            return MovementUpdate * KM_input.timepassed;
        }
    }

    internal class Numeric_Controls
    {
        private List<Keys> Float_Change_Keys = new List<Keys>(); //add, subtract
        private float reset_Value = 1f, increase_Value = 0.01f;

        private bool isResetting = false;

        internal Numeric_Controls(List<Keys> Float_Change_Keys, float reset_Value = 1f, float increase_Value = 0.01f)
        {
            Change_Keys(Float_Change_Keys);
            Change_Reset_Value(reset_Value);
            Change_increase_Value(increase_Value);
        }
        internal void Change_Keys(List<Keys> Float_Change_Keys)
        {
            this.Float_Change_Keys = Float_Change_Keys;
        }

        internal void Change_Reset_Value(float reset_Value)
        {
            this.reset_Value = reset_Value;
        }

        internal void Change_increase_Value(float increase_Value)
        {
            this.increase_Value = increase_Value;
        }

        internal float Get_Value(Keyboard_Mouse_Input KM_input, float value)
        {
            if (KM_input.KeyState.IsKeyDown(Float_Change_Keys[0]) && KM_input.KeyState.IsKeyDown(Float_Change_Keys[1]))
            {
                value = reset_Value;
                isResetting = true;
            }
            if (isResetting)
            {
                if (KM_input.KeyState.IsKeyUp(Float_Change_Keys[0]) && KM_input.KeyState.IsKeyUp(Float_Change_Keys[1]))
                    isResetting = false;
            }
            else
            {
                if (KM_input.KeyState.IsKeyDown(Float_Change_Keys[0]))
                {
                    value += increase_Value * KM_input.timepassed;
                }
                else if (KM_input.KeyState.IsKeyDown(Float_Change_Keys[1]))
                {
                    value -= increase_Value * KM_input.timepassed;
                }
            }
            return value;
        }
    }

    internal class ScrollWheel_Controls
    {
        private float increase_Value = 1f;

        internal ScrollWheel_Controls(float increase_Value = 1f)
        {
            Change_increase_Value(increase_Value);
        }

        internal void Change_increase_Value(float increase_Value)
        {
            this.increase_Value = increase_Value;
        }

        internal float Get_Update(Keyboard_Mouse_Input KM_input)
        {
            return (KM_input.MouseState.ScrollWheelValue - KM_input.PreviousMouseState.ScrollWheelValue) * increase_Value;
        }
    }

    internal class Mouse_Left_Button
    {
        private Input_Type input_Type = Input_Type.Click;
        private bool Toggled_ON = false;

        internal Mouse_Left_Button(Input_Type input_Type = Input_Type.Click)
        {
            Change_Input_Type(input_Type);
        }

        internal void Change_Input_Type(Input_Type input_Type)
        {
            this.input_Type = input_Type;
        }

        internal Point? Get_Update(Keyboard_Mouse_Input KM_input)
        {
            switch (input_Type)
            {

                case Input_Type.Click:
                    if (KM_input.MouseState.LeftButton == ButtonState.Pressed && KM_input.PreviousMouseState.LeftButton == ButtonState.Released)
                    {
                        return new Point(KM_input.MouseState.X, KM_input.MouseState.Y);
                    }
                    break;
                case Input_Type.Release:
                    if (KM_input.MouseState.LeftButton == ButtonState.Released && KM_input.PreviousMouseState.LeftButton == ButtonState.Pressed)
                    {
                        return new Point(KM_input.MouseState.X, KM_input.MouseState.Y);
                    }
                    break;
                case Input_Type.Hold:
                    if (KM_input.MouseState.LeftButton == ButtonState.Pressed)
                    {
                        return new Point(KM_input.MouseState.X, KM_input.MouseState.Y);
                    }
                    break;
                case Input_Type.Toggle:
                    if (KM_input.MouseState.LeftButton == ButtonState.Pressed && KM_input.PreviousMouseState.LeftButton == ButtonState.Released)
                    {
                        Toggled_ON = !Toggled_ON;
                    }
                    if (Toggled_ON)
                    {
                        return new Point(KM_input.MouseState.X, KM_input.MouseState.Y);
                    }
                    break;
                case Input_Type.Always:
                    return new Point(KM_input.MouseState.X, KM_input.MouseState.Y);
                default:
                    break;
            }

            return null;
        }
    }

    internal class Bool_Controls
    {
        private readonly Keys Bool_Change_Key = new Keys();

        internal Bool_Controls(Keys Bool_Change_Key)
        {
            this.Bool_Change_Key = Bool_Change_Key;
        }

        internal bool Get_Update(Keyboard_Mouse_Input KM_input, bool value)
        {
            if (KM_input.KeyState.IsKeyDown(Bool_Change_Key) && KM_input.PreviousKeyState.IsKeyUp(Bool_Change_Key))
            {
                return !value;
            }
            return value;
        }

    }
}

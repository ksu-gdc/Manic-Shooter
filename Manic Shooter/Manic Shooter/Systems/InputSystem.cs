using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Manic_Shooter;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using EntityComponentSystem.Interfaces;
using EntityComponentSystem.Components;
using EntityComponentSystem.Structure;
using EntityComponentSystem.Systems;

namespace Manic_Shooter
{
    class InputSystem:ISystem
    {
        private static InputSystem _instance;

        public static InputSystem Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new InputSystem();
                
                return _instance;
            }
        }

        /// <summary>
        /// Names for the actions that the input system can perform
        /// </summary>
        public enum Actions
        {
            MoveUp,
            MoveDown,
            MoveLeft,
            MoveRight,
            Fire,
            Slow,
            MenuUp,
            MenuDown,
            MenuLeft,
            MenuRight,
            Pause
        }

        /// <summary>
        /// The Type of action that should trigger the action
        /// </summary>
        public enum InputState
        {
            Released,
            Pressed,
            Held,
            Up
        }

        /// <summary>
        /// The state that the action can be triggered in
        /// </summary>
        public enum InputSystemState
        {
            Gameplay,
            PauseMenu
        }

        public struct ActionType
        {
            public Actions Action;
            public InputState Type;
            public InputSystemState State;

            public ActionType(Actions action, InputState type, InputSystemState state)
            {
                this.Action = action;
                this.Type = type;
                this.State = state;
            }
        }

        public Dictionary<Keys, List<ActionType>> KeyboardToActions;
        public Dictionary<GamePadButtons, List<ActionType>> GamepadToActions;

        public KeyboardState oldKeyState;
        public KeyboardState newKeyState;

        public GamePadState oldPadState;
        public GamePadState newPadState;

        public InputSystemState currentState;

        public InputSystem()
        {
            currentState = InputSystemState.Gameplay;

            KeyboardToActions = new Dictionary<Keys, List<ActionType>>();
            GamepadToActions = new Dictionary<GamePadButtons, List<ActionType>>();

            oldKeyState = newKeyState = Keyboard.GetState();
            oldPadState = newPadState = GamePad.GetState(PlayerIndex.One);

            KeyboardToActions.Add(Keys.Up, new List<ActionType>());
            KeyboardToActions.Add(Keys.Down, new List<ActionType>());
            KeyboardToActions.Add(Keys.Left, new List<ActionType>());
            KeyboardToActions.Add(Keys.Right, new List<ActionType>());
            KeyboardToActions.Add(Keys.Space, new List<ActionType>());
            KeyboardToActions.Add(Keys.Enter, new List<ActionType>());
            KeyboardToActions.Add(Keys.LeftShift, new List<ActionType>());
            KeyboardToActions.Add(Keys.RightShift, new List<ActionType>());

            KeyboardToActions[Keys.Up].Add(new ActionType(Actions.MoveUp, InputState.Pressed, InputSystemState.Gameplay));
            KeyboardToActions[Keys.Down].Add(new ActionType(Actions.MoveDown, InputState.Pressed, InputSystemState.Gameplay));
            KeyboardToActions[Keys.Left].Add(new ActionType(Actions.MoveLeft, InputState.Pressed, InputSystemState.Gameplay));
            KeyboardToActions[Keys.Right].Add(new ActionType(Actions.MoveRight, InputState.Pressed, InputSystemState.Gameplay));

            KeyboardToActions[Keys.Up].Add(new ActionType(Actions.MoveUp, InputState.Held, InputSystemState.Gameplay));
            KeyboardToActions[Keys.Down].Add(new ActionType(Actions.MoveDown, InputState.Held, InputSystemState.Gameplay));
            KeyboardToActions[Keys.Left].Add(new ActionType(Actions.MoveLeft, InputState.Held, InputSystemState.Gameplay));
            KeyboardToActions[Keys.Right].Add(new ActionType(Actions.MoveRight, InputState.Held, InputSystemState.Gameplay));

            KeyboardToActions[Keys.Up].Add(new ActionType(Actions.MenuUp, InputState.Held, InputSystemState.PauseMenu));
            KeyboardToActions[Keys.Down].Add(new ActionType(Actions.MenuDown, InputState.Held, InputSystemState.PauseMenu));
            KeyboardToActions[Keys.Left].Add(new ActionType(Actions.MenuLeft, InputState.Held, InputSystemState.PauseMenu));
            KeyboardToActions[Keys.Right].Add(new ActionType(Actions.MenuRight, InputState.Held, InputSystemState.PauseMenu));

            KeyboardToActions[Keys.Space].Add(new ActionType(Actions.Fire, InputState.Pressed, InputSystemState.Gameplay));
            KeyboardToActions[Keys.Enter].Add(new ActionType(Actions.Pause, InputState.Pressed, InputSystemState.Gameplay));

            KeyboardToActions[Keys.LeftShift].Add(new ActionType(Actions.Slow, InputState.Pressed, InputSystemState.Gameplay));
            KeyboardToActions[Keys.LeftShift].Add(new ActionType(Actions.Slow, InputState.Held, InputSystemState.Gameplay));

            KeyboardToActions[Keys.RightShift].Add(new ActionType(Actions.Slow, InputState.Pressed, InputSystemState.Gameplay));
            KeyboardToActions[Keys.RightShift].Add(new ActionType(Actions.Slow, InputState.Held, InputSystemState.Gameplay));
        }

        public InputState GetKeyState(Keys key)
        {
            if(oldKeyState.IsKeyUp(key) && newKeyState.IsKeyUp(key))
            {
                return InputState.Up;
            }
            else if(oldKeyState.IsKeyUp(key) && newKeyState.IsKeyDown(key))
            {
                return InputState.Pressed;
            }
            else if(oldKeyState.IsKeyDown(key) && newKeyState.IsKeyDown(key))
            {
                return InputState.Held;
            }
            else if(oldKeyState.IsKeyDown(key) && newKeyState.IsKeyUp(key))
            {
                return InputState.Released;
            }

            throw new Exception("Key State does not exist");
        }

        public void Update(GameTime gameTime)
        {
            newKeyState = Keyboard.GetState();
            newPadState = GamePad.GetState(PlayerIndex.One);

            foreach(Keys k in KeyboardToActions.Keys)
            {
                InputState keyState = GetKeyState(k);
                foreach(ActionType at in KeyboardToActions[k])
                {
                    if (at.State != this.currentState) continue;

                    switch(at.Action)
                    {
                        case Actions.MoveUp:
                            MoveUp(keyState);
                            break;
                        case Actions.MoveDown:
                            MoveDown(keyState);
                            break;
                        case Actions.MoveLeft:
                            MoveLeft(keyState);
                            break;
                        case Actions.MoveRight:
                            MoveRight(keyState);
                            break;
                        case Actions.Fire:
                            Fire(keyState);
                            break;
                        case Actions.Slow:
                            Slow(keyState);
                            break;
                        case Actions.Pause:
                            Pause(keyState);
                            break;
                        case Actions.MenuUp:
                            MenuUp(keyState);
                            break;
                        case Actions.MenuDown:
                            MenuDown(keyState);
                            break;
                        case Actions.MenuLeft:
                            MenuLeft(keyState);
                            break;
                        case Actions.MenuRight:
                            MenuRight(keyState);
                            break;
                    }
                }
            }

            oldKeyState = newKeyState;
            oldPadState = newPadState;
        }

        public void MoveUp(InputState buttonState)
        {
            MovementComponent MovementComponent = ComponentManagementSystem.Instance.GetComponent<MovementComponent>();
            Movement playerMovement = MovementComponent[ManicShooter.PlayerID];

            switch(buttonState)
            {
                case InputState.Up:
                    break;
                case InputState.Pressed:
                    playerMovement.VelocityVector.Y = -1;
                    break;
                case InputState.Held:
                    playerMovement.VelocityVector.Y = -1;
                    break;
                case InputState.Released:
                    playerMovement.VelocityVector.Y = 0;
                    break;
            }

            MovementComponent[ManicShooter.PlayerID] = playerMovement;
        }

        public void MoveDown(InputState buttonState)
        {
            MovementComponent MovementComponent = ComponentManagementSystem.Instance.GetComponent<MovementComponent>();
            Movement playerMovement = MovementComponent[ManicShooter.PlayerID];

            switch (buttonState)
            {
                case InputState.Up:
                    break;
                case InputState.Pressed:
                    playerMovement.VelocityVector.Y = 1;
                    break;
                case InputState.Held:
                    playerMovement.VelocityVector.Y = 1;
                    break;
                case InputState.Released:
                    playerMovement.VelocityVector.Y = 0;
                    break;
            }

            MovementComponent[ManicShooter.PlayerID] = playerMovement;
        }
        public void MoveLeft(InputState buttonState)
        {
            MovementComponent MovementComponent = ComponentManagementSystem.Instance.GetComponent<MovementComponent>();
            Movement playerMovement = MovementComponent[ManicShooter.PlayerID];

            switch (buttonState)
            {
                case InputState.Up:
                    break;
                case InputState.Pressed:
                    playerMovement.VelocityVector.X = -1;
                    break;
                case InputState.Held:
                    playerMovement.VelocityVector.X = -1;
                    break;
                case InputState.Released:
                    playerMovement.VelocityVector.X = 0;
                    break;
            }
            MovementComponent[ManicShooter.PlayerID] = playerMovement;
        }
        public void MoveRight(InputState buttonState)
        {
            MovementComponent MovementComponent = ComponentManagementSystem.Instance.GetComponent<MovementComponent>();
            Movement playerMovement = MovementComponent[ManicShooter.PlayerID];

            switch (buttonState)
            {
                case InputState.Up:
                    break;
                case InputState.Pressed:
                    playerMovement.VelocityVector.X = 1;
                    break;
                case InputState.Held:
                    playerMovement.VelocityVector.X = 1;
                    break;
                case InputState.Released:
                    playerMovement.VelocityVector.X = 0;
                    break;
            }
            MovementComponent[ManicShooter.PlayerID] = playerMovement;
        }
        public void Fire(InputState buttonState)
        {
            switch (buttonState)
            {
                case InputState.Up:
                    break;
                case InputState.Pressed:
                    break;
                case InputState.Held:
                    break;
                case InputState.Released:
                    break;
            }
        }
        public void Slow(InputState buttonState)
        {
            switch (buttonState)
            {
                case InputState.Up:
                    break;
                case InputState.Pressed:
                    break;
                case InputState.Held:
                    break;
                case InputState.Released:
                    break;
            }
        }
        public void Pause(InputState buttonState)
        {
            switch (buttonState)
            {
                case InputState.Up:
                    break;
                case InputState.Pressed:
                    break;
                case InputState.Held:
                    break;
                case InputState.Released:
                    break;
            }
        }
        public void MenuUp(InputState buttonState)
        {
            switch (buttonState)
            {
                case InputState.Up:
                    break;
                case InputState.Pressed:
                    break;
                case InputState.Held:
                    break;
                case InputState.Released:
                    break;
            }
        }
        public void MenuDown(InputState buttonState)
        {
            switch (buttonState)
            {
                case InputState.Up:
                    break;
                case InputState.Pressed:
                    break;
                case InputState.Held:
                    break;
                case InputState.Released:
                    break;
            }
        }
        public void MenuLeft(InputState buttonState)
        {
            switch (buttonState)
            {
                case InputState.Up:
                    break;
                case InputState.Pressed:
                    break;
                case InputState.Held:
                    break;
                case InputState.Released:
                    break;
            }
        }
        public void MenuRight(InputState buttonState)
        {
            switch (buttonState)
            {
                case InputState.Up:
                    break;
                case InputState.Pressed:
                    break;
                case InputState.Held:
                    break;
                case InputState.Released:
                    break;
            }
        }
    }
}

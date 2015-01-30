﻿using SkypeBot.InputDevices;

namespace SkypeBot.WindowsAPI
{
    public class InputFactory
    {
        public static Input Mouse(MouseInput mouseInput)
        {
            return Input.MouseInput(WindowsConstants.INPUT_MOUSE, mouseInput);
        }

        public static Input Keyboard(KeyboardInput keyboardInput)
        {
            return Input.KeyboardInput(WindowsConstants.INPUT_KEYBOARD, keyboardInput);
        }
    }
}
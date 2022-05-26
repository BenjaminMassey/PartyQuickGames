using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class JoyCon
{
    // https://www.reddit.com/r/Unity3D/comments/66elwn/can_the_switch_joycon_be_used_in_unity_on_pc/dgi6xsv/

    public static KeyCode A() { return KeyCode.JoystickButton0; }
    public static KeyCode A(int joystick_num) { return Str2Key("Joystick" + joystick_num.ToString() + "Button0"); }

    public static KeyCode B() { return KeyCode.JoystickButton2; }
    public static KeyCode B(int joystick_num) { return Str2Key("Joystick" + joystick_num.ToString() + "Button2"); }

    public static KeyCode X() { return KeyCode.JoystickButton1; }
    public static KeyCode X(int joystick_num) { return Str2Key("Joystick" + joystick_num.ToString() + "Button1"); }

    public static KeyCode Y() { return KeyCode.JoystickButton3; }
    public static KeyCode Y(int joystick_num) { return Str2Key("Joystick" + joystick_num.ToString() + "Button3"); }

    public static KeyCode Plus() { return KeyCode.JoystickButton9; }
    public static KeyCode Plus(int joystick_num) { return Str2Key("Joystick" + joystick_num.ToString() + "Button9"); }
    
    public static KeyCode Minus() { return KeyCode.JoystickButton8; }
    public static KeyCode Minus(int joystick_num) { return Str2Key("Joystick" + joystick_num.ToString() + "Button9"); }

    public static KeyCode Home() { return KeyCode.JoystickButton12; }
    public static KeyCode Home(int joystick_num) { return Str2Key("Joystick" + joystick_num.ToString() + "Button12"); }


    public static KeyCode R() { return KeyCode.JoystickButton14; }
    public static KeyCode R(int joystick_num) { return Str2Key("Joystick" + joystick_num.ToString() + "Button14"); }

    public static KeyCode ZR() { return KeyCode.JoystickButton15; }
    public static KeyCode ZR(int joystick_num) { return Str2Key("Joystick" + joystick_num.ToString() + "Button15"); }


    public static KeyCode SR() { return KeyCode.JoystickButton5; }
    public static KeyCode SR(int joystick_num) { return Str2Key("Joystick" + joystick_num.ToString() + "Button5"); }

    public static KeyCode SL() { return KeyCode.JoystickButton4; }
    public static KeyCode SL(int joystick_num) { return Str2Key("Joystick" + joystick_num.ToString() + "Button4"); }

    public static string StickX() { return "JoyCon Horizontal"; } // Joystick Horizontal = Joystick Axis 10.
    public static string StickX(int joystick_num) { return "JoyCon " + joystick_num.ToString() + " Horizontal"; }

    public static string StickY() { return "JoyCon Vertical"; } // Joystick Vertical = Joystick Axis 9.
    public static string StickY(int joystick_num) { return "JoyCon " + joystick_num.ToString() + " Vertical"; }

    // http://answers.unity.com/answers/653113/view.html
    private static KeyCode Str2Key(string s) { return (KeyCode)System.Enum.Parse(typeof(KeyCode), s); }
}

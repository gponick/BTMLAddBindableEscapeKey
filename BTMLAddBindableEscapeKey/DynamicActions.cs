﻿using BattleTech;
using BattleTech.UI;
using Harmony;
using InControl;
using System;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BattleTech.Data;
using BattleTech.UI.Tooltips;
using HBS;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace BTMLAddBindableEscapeKey
{
    public class KeyBinding
    {
        public class Adapter<T>
        {
            public readonly T instance;
            public readonly Traverse traverse;

            protected Adapter(T instance)
            {
                this.instance = instance;
                traverse = Traverse.Create(instance);
            }
        }

        [HarmonyPatch(typeof(StaticActions), "CreateWithDefaultBindings")]
        public static class StaticActions_Patch_CreateWDB
        {
            public static void Postfix(ref StaticActions __result)
            {
                Logger.LogLine("Clearing Default Bindings for 'Escape' Key");
                __result.Escape.ClearBindings();
                Logger.LogLine("Setting Escape key as defaults for 'Escape'");
                __result.Escape.AddDefaultBinding(new Key[]
                {
                    Key.Escape
                });
                Logger.LogLine("Success Escape key as defaults for 'Escape'");
                if (BTMLAddBindableEscapeKey.ModSettings.EnableSpaceKey)
                {
                    Logger.LogLine("Setting Space Default for Escape");
                    __result.Escape.AddDefaultBinding(new Key[]
                    {
                        Key.Space
                    });
                    Logger.LogLine("Success Space Default for Escape");
                }
                if (BTMLAddBindableEscapeKey.ModSettings.EnableMouseButton)
                { 
                    Logger.LogLine($"Setting Mouse{BTMLAddBindableEscapeKey.ModSettings._escapeMouseButtonInput} default for escape");
                    __result.Escape.AddDefaultBinding(new MouseBindingSource(BTMLAddBindableEscapeKey.ModSettings.EscapeMouseButton));
                    Logger.LogLine("New Mouse escape defaults set!");

                    foreach(int mouseButtonInt in BTMLAddBindableEscapeKey.ModSettings.__additionalMouseButtons)
                    {
                        if (mouseButtonInt >= 2 && mouseButtonInt <= 9)
                        {
                            Mouse mouseButtonFromInt = SettingsMouseHelper.GetButtonFromInt(mouseButtonInt);
                            Logger.LogLine($"Setting Mouse{mouseButtonInt} default for escape via __additionalMouseButtons");
                            __result.Escape.AddDefaultBinding(new MouseBindingSource(mouseButtonFromInt));
                            Logger.LogLine($"Success Mouse{mouseButtonInt} default for escape via __aditionalMouseButtons");
                        }
                    }
                }
            }
        }
    }
}
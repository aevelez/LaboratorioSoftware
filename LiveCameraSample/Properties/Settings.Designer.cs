﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace LiveCameraSample.Properties {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "16.7.0.0")]
    internal sealed partial class Settings : global::System.Configuration.ApplicationSettingsBase {
        
        private static Settings defaultInstance = ((Settings)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new Settings())));
        
        public static Settings Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("540f5628a89945699eeccf5bd50aaca4")]
        public string FaceAPIKey {
            get {
                return ((string)(this["FaceAPIKey"]));
            }
            set {
                this["FaceAPIKey"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("")]
        public string VisionAPIKey {
            get {
                return ((string)(this["VisionAPIKey"]));
            }
            set {
                this["VisionAPIKey"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("Visible")]
        public global::System.Windows.Visibility SettingsPanelVisibility {
            get {
                return ((global::System.Windows.Visibility)(this["SettingsPanelVisibility"]));
            }
            set {
                this["SettingsPanelVisibility"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("00:00:03")]
        public global::System.TimeSpan AnalysisInterval {
            get {
                return ((global::System.TimeSpan)(this["AnalysisInterval"]));
            }
            set {
                this["AnalysisInterval"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("0")]
        public int FaceAPICallCount {
            get {
                return ((int)(this["FaceAPICallCount"]));
            }
            set {
                this["FaceAPICallCount"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("0")]
        public int VisionAPICallCount {
            get {
                return ((int)(this["VisionAPICallCount"]));
            }
            set {
                this["VisionAPICallCount"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("True")]
        public bool AutoStopEnabled {
            get {
                return ((bool)(this["AutoStopEnabled"]));
            }
            set {
                this["AutoStopEnabled"] = value;
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("00:05:00")]
        public global::System.TimeSpan AutoStopTime {
            get {
                return ((global::System.TimeSpan)(this["AutoStopTime"]));
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("https://westus.api.cognitive.microsoft.com")]
        public string FaceAPIHost {
            get {
                return ((string)(this["FaceAPIHost"]));
            }
            set {
                this["FaceAPIHost"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("https://westus.api.cognitive.microsoft.com")]
        public string VisionAPIHost {
            get {
                return ((string)(this["VisionAPIHost"]));
            }
            set {
                this["VisionAPIHost"] = value;
            }
        }
    }
}
#region Using directives
using UAManagedCore;
using OpcUa = UAManagedCore.OpcUa;
using FTOptix.Alarm;
using FTOptix.HMIProject;
using FTOptix.Core;
using FTOptix.CoreBase;
using FTOptix.NetLogic;
using FTOptix.UI;
using System.Collections.Generic;
using System;
using System.Linq;
using NetlogicTesting.shared;
using FTOptix.WebUI;
#endregion


public class BasicDigitalAlarmRT : BaseNetLogic
{
    [ExportMethod]
    public void generateProject()
    {
        // Remove Default UI folder
        var projectRoot = Project.Current;
        var ui_folder =  Owner.Get("UI");
        var opc_folder = Owner.Get("OPC-UA");
        var logger_folder = Owner.Get("DataStores");
        var reports_folder = Owner.Get("Reports");
        var converters_folder = Owner.Get("Converters");
        var alarm_folder = Owner.Get("Alarms");
        var model_folder = Owner.Get("Model");
        projectRoot.Remove(alarm_folder);
        projectRoot.Remove(model_folder);
        projectRoot.Remove(ui_folder);
        projectRoot.Remove(logger_folder);
        projectRoot.Remove(opc_folder);
        projectRoot.Remove(reports_folder);
        projectRoot.Remove(converters_folder);

        // Generate Folders and base project objects
        ProjectCreation.generateBase();

        // Get project
        var mainWindow = projectRoot.Get("UI").Get("MainWindow");
        var window = Owner.Get("UI").Get<WindowType>("MainWindow");
        window.Width = 1300;

        // Create Model folder
        var model = InformationModel.MakeObject<Folder>("Model");
        projectRoot.Add(model);

        // Create Alarms folder
        var alarms = InformationModel.MakeObject<Folder>("Alarms");
        projectRoot.Add(alarms);


        // Insert code to be executed when the user-defined logic is started
        // Create Alarms
        var digitalAlarm1 = InformationModel.MakeObject<DigitalAlarm>("DigitalAlarm1");
        digitalAlarm1.Message = "Something is alarming";
        alarms.Add(digitalAlarm1);

        var digitalAlarm2 = InformationModel.MakeObject<DigitalAlarm>("DigitalAlarm2");
        digitalAlarm2.Message = "Something else is alarming";
        alarms.Add(digitalAlarm2);

        var digitalAlarm3 = InformationModel.MakeObject<DigitalAlarm>("DigitalAlarm3");
        digitalAlarm3.Message = "Something more is alarming";
        digitalAlarm3.NormalStateValue = 5;
        alarms.Add(digitalAlarm3);


        // Create Variables
        var variable1 = InformationModel.MakeVariable("Variable1", OpcUa.DataTypes.Boolean);
        model.Add(variable1);

        var variable2 = InformationModel.MakeVariable("Variable2", OpcUa.DataTypes.Boolean);
        model.Add(variable2);

        var variable3 = InformationModel.MakeVariable("Variable3", OpcUa.DataTypes.Int32);
        variable3.Value = 5;
        model.Add(variable3);


        // Set alarms dynamic links
        digitalAlarm1.InputValueVariable.SetDynamicLink(variable1);
        digitalAlarm2.InputValueVariable.SetDynamicLink(variable2);
        digitalAlarm3.InputValueVariable.SetDynamicLink(variable3, DynamicLinkMode.ReadWrite);

        // Create Switches
        var switch_1 = InformationModel.MakeObject<Switch>("Switch1");
        switch_1.BottomMargin = 50;
        switch_1.LeftMargin = 50;
        switch_1.Height = 40;
        switch_1.Width = 80;
        switch_1.HorizontalAlignment = HorizontalAlignment.Left;
        switch_1.VerticalAlignment = VerticalAlignment.Bottom;
        switch_1.CheckedText = "ON";
        switch_1.UncheckedText = "OFF";
        switch_1.Checked = false;
        mainWindow.Add(switch_1);

        var switch_2 = InformationModel.MakeObject<Switch>("Switch2");
        switch_2.BottomMargin = 50;
        switch_2.LeftMargin = 150;
        switch_2.Height = 40;
        switch_2.Width = 80;
        switch_2.HorizontalAlignment = HorizontalAlignment.Left;
        switch_2.VerticalAlignment = VerticalAlignment.Bottom;
        switch_2.CheckedText = "ON";
        switch_2.UncheckedText = "OFF";
        switch_2.Checked = false;
        mainWindow.Add(switch_2);

        var switch_3 = InformationModel.MakeObject<Switch>("Switch3");
        switch_3.BottomMargin = 50;
        switch_3.LeftMargin = 250;
        switch_3.Height = 40;
        switch_3.Width = 80;
        switch_3.HorizontalAlignment = HorizontalAlignment.Left;
        switch_3.VerticalAlignment = VerticalAlignment.Bottom;
        switch_3.CheckedText = "ON";
        switch_3.UncheckedText = "OFF";
        switch_3.Checked = false;
        mainWindow.Add(switch_3);

        // Create TextBox
        var textBox1 = InformationModel.MakeObject<TextBox>("Textbox1");
        textBox1.BottomMargin = 50;
        textBox1.LeftMargin = 350;
        textBox1.Height = 20;
        textBox1.Width = 80;
        textBox1.HorizontalAlignment = HorizontalAlignment.Left;
        textBox1.VerticalAlignment = VerticalAlignment.Bottom;
        textBox1.TextVerticalAlignment = TextVerticalAlignment.Bottom;
        textBox1.ValueChangeBehaviour = ValueChangeBehaviour.ValueChangeWhileEditing;
        mainWindow.Add(textBox1);

        // Add Dynamic links
        switch_1.CheckedVariable.SetDynamicLink(variable1, DynamicLinkMode.ReadWrite);
        switch_2.CheckedVariable.SetDynamicLink(variable2, DynamicLinkMode.ReadWrite);
        switch_3.CheckedVariable.SetDynamicLink(digitalAlarm2.EnabledVariable, DynamicLinkMode.ReadWrite);
        textBox1.TextVariable.SetDynamicLink(variable3, DynamicLinkMode.ReadWrite);

        Log.Info("Project Generation Complete");
    }
}

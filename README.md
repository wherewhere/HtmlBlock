# HtmlBlock
A control to parse and render markdown for UWP. Base on the [HtmlBlock](https://github.com/wasteam/waslibs/tree/master/src/AppStudio.Uwp/Controls/HtmlBlock) control of [Windows App Studio](https://github.com/wasteam/waslibs).

[![Issues](https://img.shields.io/github/issues/wherewhere/HtmlBlock.svg?label=Issues&style=flat-square)](https://github.com/wherewhere/HtmlBlock/issues "Issues")
[![Stargazers](https://img.shields.io/github/stars/wherewhere/HtmlBlock.svg?label=Stars&style=flat-square)](https://github.com/wherewhere/HtmlBlock/stargazers "Stargazers")

## Support Platform
- UAP 10.0.10240.0

## Example
```xaml
<Page
    x:Class="HtmlBlock.SampleApp.MainPage"
    xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
    xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
    xmlns:controls="using:HtmlBlock.SampleApp.Controls"
    xmlns:d="http://schemas.microsoft.com/expression/blend/2008"
    xmlns:htmlblock="using:HtmlBlock"
    xmlns:mc="http://schemas.openxmlformats.org/markup-compatibility/2006"
    Background="{ThemeResource ApplicationPageBackgroundBrush}"
    mc:Ignorable="d">
    <Grid Grid.Row="1">
        <Grid.RowDefinitions>
            <RowDefinition Height="*" />
            <RowDefinition Height="Auto" />
        </Grid.RowDefinitions>
        <controls:TwoPaneView
            x:Name="TwoPaneView"
            Grid.Row="0"
            MinTallModeHeight="NaN"
            MinWideModeWidth="NaN"
            Pane1Length="*"
            Pane2Length="*">
            <controls:TwoPaneView.Pane1>
                <TextBox
                    x:Name="TextBox"
                    Margin="10"
                    AcceptsReturn="True"
                    TextWrapping="Wrap" />
            </controls:TwoPaneView.Pane1>
            <controls:TwoPaneView.Pane2>
                <htmlblock:TextBlockEx
                    Margin="16"
                    Text="{x:Bind TextBox.Text, Mode=OneWay}" />
            </controls:TwoPaneView.Pane2>
        </controls:TwoPaneView>
        <CommandBar
            x:Name="CommandBar"
            Grid.Row="1"
            VerticalAlignment="Bottom" />
    </Grid>
</Page>

```
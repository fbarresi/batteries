# batteries.Email ![logo](https://raw.githubusercontent.com/gammasoft/fatcow/refs/heads/master/32x32/battery_charge.png)

[![NuGet Version](https://img.shields.io/nuget/vpre/batteries.Disposables)](https://www.nuget.org/packages/batteries.Disposables)

## Description

This package contains a useful disposable implementation based on [System.Reactive](https://github.com/dotnet/reactive).
It is based on a fork of the main repository and allow the usage of disposables even without importing the whole system.reactive package.

## Usage

The full usage can be read [here](https://introtorx.com/chapters/disposables)

#### Disclaimer
ContextDisposable and ScheduledDisposable were not imported.
If you need them => install the package system.reactive. 
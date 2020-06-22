# Wox plugin ghq

Wox plugin for [ghq](https://github.com/x-motemen/ghq)

## Install

1. Download wox-plugin-ghq.wox from https://github.com/mtsmfm/wox-plugin-ghq/releases
2. Drag and drop wox-plugin-ghq.wox into wox toolbar

## Usage

Action keyword is `ghq`.

## Configuration

You can find setting file in C:\Users\USERNAME\Documents\wox-plugin-ghq.json.

```jsonc
{
    "shell": "wsl.exe", // or like "cmd.exe"
    "ghqCommand": "/home/foo/.bin/ghq", // Path to ghq
    "openCommand": "explorer.exe" // or like "code"
}
```

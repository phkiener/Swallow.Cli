# Swallow.Cli

An experiment.

## Roadmap

- Simple routing for delegates
  - Commands
  - Flags / Switches (`-b` / `--bar`)
  - Options (`-b FOO` / `--bar FOO`)
  - Arguments
- Advanced routing
  - Compound options (`-bar foo` => `-b -a -r foo`)
  - Double-dash (`-f -- -b` => `-b` as argument)
  - Windows-style (`/` instead of `-`, PascalCase instead of kebap-case)
  - Binding options & arguments to custom types
- Controllers as entrypoints instead of delegates
- Dynamic access to the list of entrypoints
- Configuration inside `$XDG_CONFIG_HOME` (and other `XDG_*`-dirs?)
- Out-of-the-box help (`-h`, `--help`) and version (`-v`, `--version`) entrypoints

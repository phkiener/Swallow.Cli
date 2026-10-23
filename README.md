# Swallow.Cli

An experiment.

## Roadmap

- Simple routing for delegates
  - [x] Commands
  - [x] Flags / Switches (`-b` / `--bar`)
  - [x] Options (`-b FOO` / `--bar FOO`)
  - [x] Arguments
- Advanced routing
  - [ ] Compound options (`-bar foo` => `-b -a -r foo`)
  - [x] Double-dash (`-f -- -b` => `-b` as argument)
  - [ ] Windows-style (`/` instead of `-`, PascalCase instead of kebap-case)
  - [ ] Binding options & arguments to custom types
  - [ ] Bind list to options and arguments
  - [ ] "Accept all"-style parameters
- [ ] "Controllers" (name pending) as entrypoints instead of delegates
- [ ] Dynamic access to the list of entrypoints (think `EndpointDataSource`)
- [ ] Configuration inside `$XDG_CONFIG_HOME` (and other `XDG_*`-dirs?)
- [ ] Out-of-the-box help (`-h`, `--help`) and version (`-v`, `--version`) entrypoints

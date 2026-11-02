# Swallow.Cli

An experiment.

## Roadmap

- Simple routing for delegates
  - [x] Commands
  - [x] Flags / Switches (`-b` / `--bar`)
  - [x] Options (`-b FOO` / `--bar FOO`)
  - [x] Arguments
- Advanced routing
  - [x] Compound options (`-bar foo` => `-b -a -r foo`)
  - [x] Double-dash (`-f -- -b` => `-b` as argument)
  - [ ] Windows-style (`/` instead of `-`, PascalCase instead of kebap-case)
  - [x] Binding options & arguments to custom types
  - [ ] Bind list to options and arguments (`StringValues`)
  - [ ] "Accept all"-style parameters
- [ ] "Controllers" (name pending) as entrypoints instead of delegates
- [ ] Configurable routing (/ invocation?) middlewares
- [ ] Out-of-the-box help (`-h`, `--help`) and version (`-v`, `--version`) entrypoints

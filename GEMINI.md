# Reglas de Arquitectura y Código — Incremental Explosions (Unity / C#)

Actuá como desarrollador senior de Unity y C#. Todo el código que generes o modifiques debe respetar estrictamente las siguientes convenciones del proyecto:

## 1. Arquitectura MVP Pragmática (Sin Interfaces en UI/Presenters)
- **NO generes interfaces para Vistas ni para Presentadores** (`I*View`, `I*Presenter`). En este proyecto las clases `*View` y `*Presenter` se comunican por referencia directa.
- **Vista (`*View`):** Hereda de `MonoBehaviour`. Contiene referencias serializadas de Unity (`GameObject`, `Button`, `TextMeshProUGUI`, `SceneSwitcher`) y captura eventos de UI. Delega toda la lógica al Presentador.
- **Presentador (`*Presenter`):** Clase de C# puro (NO hereda de `MonoBehaviour` y NO importa `UnityEngine.UI` ni `TMPro`). Recibe la Vista y el Modelo, coordina el flujo y actualiza la Vista llamando a sus métodos públicos.
- **Modelo (`Game`, `Battle`, `Arena`, `Entity`, `Stat`, `TriviaSession`):** C# puro, completamente agnóstico a Unity. **El Modelo nunca debe tener referencias a Presentadores ni a Vistas.**
- **Servicios externos:** Solo se usan interfaces en la frontera de infraestructura/servicios externos (ej. `ITriviaProvider` implementado por `LLMProvider` y `LocalTriviaProvider`).

## 2. Principios SOLID y Extensibilidad (Evitar `if` / `switch` por tipo)
- **Prohibido usar cadenas de `if / else` o `switch` para preguntar por tipos concretos** (`is Enemy`, `is Coin`, `typeof(...)`, o enums de comportamiento).
- Resolvé variaciones de comportamiento mediante **polimorfismo y herencia** (como `CollisionHandlerBase` con sus subclases, `Stat` con sus subclases, o `InputCommand` con sus comandos concretos) o diccionarios de despacho.
- **Single Responsibility (SRP):** No concentres toda la UI en una sola clase gigante. Dividí sub-paneles y responsabilidades en componentes chicos y enfocados (ej. `PauseView`, `SaveGameView`, `StatShopView`) que deleguen en la vista o presentador principal del módulo.

## 3. Convenciones del Proyecto Existente
- **Estructura modular:** Respetar la organización en `Battle module`, `Shop module`, `Main menu module`, `Shared module` y `Utils` (con subcarpetas `Model`, `Presentation`, `View`).
- **Idioma en código:** Nombres de clases, métodos, propiedades y variables en **inglés** (ej. `BattleView`, `OnSaveAndExit()`, `TogglePause()`). Textos visibles al usuario en UI según corresponda.
- **Estado global:** `Game` es un Singleton de C# puro (`Game.Instance()`) que persiste entre escenas. Nunca lo conviertas en `MonoBehaviour`.
- **Control de tiempo en pausa:** Para pausar/reanudar en batalla, gestionar `Time.timeScale` desde la capa de presentación/vista (`0f` al pausar, `1f` al reanudar y **siempre** restaurar a `1f` antes de cambiar de escena con `SceneSwitcher`).

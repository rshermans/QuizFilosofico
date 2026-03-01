# Quiz Filosófico (Refactored)

This project has been transformed from a static .NET quiz app into a dynamic **Python FastAPI Philosophical Engine**.

## Vision
Inspired by Socratic dialogue and modern agentic systems (like VisionClaw), this platform offers:
1.  **Socratic Arena**: Interactive debates with AI personas (e.g., Socrates, Nietzsche).
2.  **The Claw (Logic Referee)**: A real-time background agent that analyzes your arguments for logical fallacies.
3.  **Argument Recovery**: The system remembers your claims (in-memory for now) to challenge contradictions.

## Architecture

-   **Backend**: Python FastAPI
-   **AI Engine**: OpenAI API (GPT-3.5/4) via `SocraticAgent` and `ClawService`.
-   **Frontend**: HTML5, TailwindCSS (CDN), Vanilla JS.
-   **Legacy Code**: The original C# project is archived in `legacy_dotnet/`.

## Prerequisites

-   Python 3.9+
-   An OpenAI API Key

## Installation

1.  Clone the repository.
2.  Install dependencies:
    ```bash
    pip install -r requirements.txt
    ```
3.  Set up your environment variables:
    ```bash
    export OPENAI_API_KEY="sk-..."
    ```
    (Or create a `.env` file)

## Usage

Run the server:
```bash
./run.sh
```

Open your browser at `http://localhost:8000`.

## Features
-   [x] Real-time Socratic Dialogue
-   [x] "The Claw" Fallacy Detection
-   [ ] Persistent Debate History (Database integration pending)
-   [ ] Voice Input (Future work)

## Legacy
The original .NET application is preserved in `legacy_dotnet/` for reference.

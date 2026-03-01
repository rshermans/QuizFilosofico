import openai
from typing import List
from app.core.config import settings
from app.models.schemas import ChatMessage

class SocraticAgent:
    def __init__(self, persona: str = "Socrates"):
        self.persona = persona
        self.client = openai.OpenAI(api_key=settings.OPENAI_API_KEY)

    def generate_response(self, history: List[ChatMessage]) -> str:
        system_prompt = {
            "role": "system",
            "content": f"You are {self.persona}. Engage the user in a Socratic dialogue. challenge their assumptions, ask probing questions, and do not give direct answers. Keep responses concise and philosophical."
        }

        messages = [system_prompt] + [msg.model_dump() for msg in history]

        try:
            response = self.client.chat.completions.create(
                model="gpt-3.5-turbo",
                messages=messages
            )
            return response.choices[0].message.content
        except Exception as e:
            return f"Error interacting with the oracle: {str(e)}"

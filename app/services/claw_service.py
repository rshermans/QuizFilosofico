import openai
from app.core.config import settings
from app.models.claw_schemas import ArgumentAnalysis
import json
import logging

logger = logging.getLogger(__name__)

class ClawService:
    def __init__(self):
        self.client = openai.OpenAI(api_key=settings.OPENAI_API_KEY)

    def detect_fallacies(self, text: str) -> ArgumentAnalysis:
        system_prompt = {
            "role": "system",
            "content": """
            You are a Logic Referee (The Claw). Analyze the user's argument for logical fallacies.
            Return the output in strictly valid JSON format matching this schema:
            {
                "fallacies": [{"name": "string", "description": "string", "confidence": float}],
                "summary": "string",
                "counter_point": "string or null"
            }
            If no fallacy is found, return an empty list for "fallacies".
            """
        }

        user_message = {"role": "user", "content": text}

        try:
            response = self.client.chat.completions.create(
                model="gpt-3.5-turbo",
                messages=[system_prompt, user_message],
                temperature=0.3
            )
            content = response.choices[0].message.content
            # Basic cleanup if the model wraps json in markdown
            content = content.strip()
            if content.startswith("```json"):
                content = content[7:]
            if content.endswith("```"):
                content = content[:-3]

            data = json.loads(content)
            return ArgumentAnalysis(**data)
        except Exception as e:
            logger.error(f"Error in ClawService: {e}")
            return ArgumentAnalysis(
                fallacies=[],
                summary=f"Error analyzing argument: {str(e)}",
                counter_point=None
            )

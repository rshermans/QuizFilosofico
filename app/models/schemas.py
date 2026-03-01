from typing import List, Optional
from pydantic import BaseModel

class ChatMessage(BaseModel):
    role: str
    content: str

class QuizRequest(BaseModel):
    topic: str
    persona: Optional[str] = "Socrates"

class QuizResponse(BaseModel):
    response: str
    history: List[ChatMessage]

class Argument(BaseModel):
    text: str
    context: Optional[str] = None

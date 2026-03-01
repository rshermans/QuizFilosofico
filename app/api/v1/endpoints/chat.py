from fastapi import APIRouter, HTTPException
from typing import List
from app.models.schemas import ChatMessage, QuizResponse
from app.services.ai_service import SocraticAgent

router = APIRouter()
agent = SocraticAgent()

@router.post("/chat", response_model=QuizResponse)
async def chat_endpoint(history: List[ChatMessage]):
    if not history:
        raise HTTPException(status_code=400, detail="Chat history is required")

    response_text = agent.generate_response(history)

    # Append the agent's response to the history to return it
    new_history = history.copy()
    new_history.append(ChatMessage(role="assistant", content=response_text))

    return QuizResponse(response=response_text, history=new_history)

from fastapi import APIRouter
from app.api.v1.endpoints import claw, chat

api_router = APIRouter()
api_router.include_router(claw.router, tags=["claw"])
api_router.include_router(chat.router, tags=["chat"])

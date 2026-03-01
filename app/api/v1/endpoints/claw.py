from fastapi import APIRouter, HTTPException
from app.models.schemas import Argument
from app.models.claw_schemas import ArgumentAnalysis
from app.services.claw_service import ClawService

router = APIRouter()
claw_service = ClawService()

@router.post("/analyze_argument", response_model=ArgumentAnalysis)
async def analyze_argument_endpoint(argument: Argument):
    if not argument.text:
        raise HTTPException(status_code=400, detail="Argument text is required")

    return claw_service.detect_fallacies(argument.text)

from typing import List, Optional
from pydantic import BaseModel

class Fallacy(BaseModel):
    name: str
    description: str
    confidence: float

class ArgumentAnalysis(BaseModel):
    fallacies: List[Fallacy]
    summary: str
    counter_point: Optional[str] = None

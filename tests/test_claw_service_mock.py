from unittest.mock import MagicMock, patch
from app.services.claw_service import ClawService
from app.models.claw_schemas import ArgumentAnalysis
import json

def test_claw_service_mock():
    # Mock OpenAI client response
    mock_data = {
        "fallacies": [
            {"name": "Ad Hominem", "description": "Attacking the person instead of the argument", "confidence": 0.9}
        ],
        "summary": "The user attacked the opponent personally.",
        "counter_point": "Focus on the logic, not the person."
    }
    mock_json_response = json.dumps(mock_data)

    with patch("app.services.claw_service.openai.OpenAI") as MockClient:
        mock_instance = MockClient.return_value
        mock_completion = MagicMock()
        mock_completion.choices = [MagicMock(message=MagicMock(content=mock_json_response))]
        mock_instance.chat.completions.create.return_value = mock_completion

        service = ClawService()
        result = service.detect_fallacies("You are wrong because you are ugly.")

        assert isinstance(result, ArgumentAnalysis)
        assert len(result.fallacies) == 1
        assert result.fallacies[0].name == "Ad Hominem"
        assert result.counter_point == "Focus on the logic, not the person."

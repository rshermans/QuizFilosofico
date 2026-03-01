from unittest.mock import MagicMock, patch
from app.services.ai_service import SocraticAgent
from app.models.schemas import ChatMessage

def test_socratic_agent_mock():
    # Mock OpenAI client
    with patch("app.services.ai_service.openai.OpenAI") as MockClient:
        mock_instance = MockClient.return_value
        mock_completion = MagicMock()
        mock_completion.choices = [MagicMock(message=MagicMock(content="I know that I know nothing."))]
        mock_instance.chat.completions.create.return_value = mock_completion

        agent = SocraticAgent(persona="Socrates")
        history = [ChatMessage(role="user", content="What is truth?")]

        response = agent.generate_response(history)

        assert response == "I know that I know nothing."
        mock_instance.chat.completions.create.assert_called_once()

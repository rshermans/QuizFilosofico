document.addEventListener('DOMContentLoaded', () => {
    const chatForm = document.getElementById('chat-form');
    const userInput = document.getElementById('user-input');
    const chatBox = document.getElementById('chat-box');
    const clawPanel = document.getElementById('claw-panel');

    let history = [];

    chatForm.addEventListener('submit', async (e) => {
        e.preventDefault();
        const text = userInput.value.trim();
        if (!text) return;

        // Add user message
        appendMessage('User', text, 'right');
        userInput.value = '';

        // 1. Analyze with Claw (Parallel)
        analyzeArgument(text);

        // 2. Get Agent Response
        // Ideally this would be a real API call to the agent service we built
        // For MVP we mock or implement a simple endpoint
        await getAgentResponse(text);
    });

    function appendMessage(sender, text, align) {
        const div = document.createElement('div');
        div.className = `flex justify-${align === 'right' ? 'end' : 'start'}`;

        const bubble = document.createElement('div');
        bubble.className = align === 'right'
            ? 'bg-gray-700 p-3 rounded-lg max-w-2xl border border-gray-600'
            : 'bg-blue-900/50 p-3 rounded-lg max-w-2xl border border-blue-800';

        const name = document.createElement('p');
        name.className = `text-sm font-semibold mb-1 ${align === 'right' ? 'text-gray-300' : 'text-blue-300'}`;
        name.textContent = sender;

        const content = document.createElement('p');
        content.textContent = text;

        bubble.appendChild(name);
        bubble.appendChild(content);
        div.appendChild(bubble);
        chatBox.appendChild(div);
        chatBox.scrollTop = chatBox.scrollHeight;
    }

    async function analyzeArgument(text) {
        try {
            const response = await fetch('/api/v1/analyze_argument', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({ text: text })
            });

            if (response.ok) {
                const data = await response.json();
                updateClawPanel(data);
            }
        } catch (error) {
            console.error('Claw error:', error);
        }
    }

    function updateClawPanel(data) {
        // Clear "No fallacies" text if it exists
        if (clawPanel.querySelector('.italic')) {
            clawPanel.innerHTML = '';
        }

        if (data.fallacies && data.fallacies.length > 0) {
            data.fallacies.forEach(fallacy => {
                const card = document.createElement('div');
                card.className = 'bg-red-900/30 border border-red-800 p-3 rounded-lg animate-fade-in';

                card.innerHTML = `
                    <div class="flex justify-between items-start mb-1">
                        <h3 class="font-bold text-red-400 text-sm">${fallacy.name}</h3>
                        <span class="text-xs bg-red-900 text-red-200 px-1 rounded">${(fallacy.confidence * 100).toFixed(0)}%</span>
                    </div>
                    <p class="text-xs text-gray-300">${fallacy.description}</p>
                `;
                clawPanel.prepend(card);
            });
        }

        if (data.counter_point) {
             const tip = document.createElement('div');
             tip.className = 'bg-yellow-900/20 border border-yellow-800 p-3 rounded-lg mt-2';
             tip.innerHTML = `<p class="text-xs text-yellow-500">💡 <strong>Counter Point:</strong> ${data.counter_point}</p>`;
             clawPanel.prepend(tip);
        }
    }

    async function getAgentResponse(userText) {
        const loadingDiv = document.createElement('div');
        loadingDiv.className = 'flex justify-start';
        loadingDiv.innerHTML = '<div class="bg-blue-900/50 p-3 rounded-lg"><p class="text-blue-300 text-sm">Thinking...</p></div>';
        chatBox.appendChild(loadingDiv);

        history.push({role: "user", content: userText});

        try {
            const response = await fetch('/api/v1/chat', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify(history)
            });

            chatBox.removeChild(loadingDiv);

            if (response.ok) {
                const data = await response.json();
                history = data.history;
                appendMessage('Socrates', data.response, 'left');
            } else {
                appendMessage('System', 'Failed to communicate with the oracle.', 'left');
            }
        } catch (error) {
            chatBox.removeChild(loadingDiv);
            console.error('Agent error:', error);
            appendMessage('System', 'Error connecting to the oracle.', 'left');
        }
    }
});

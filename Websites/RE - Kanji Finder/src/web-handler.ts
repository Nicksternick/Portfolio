// ===== | Methods | =====

/** Takes a URL to a web service and returns the JSON */
const fetchJson = async (url: string) => {
    // Return the result of fect dependin on if it succeed and fail
    return fetch(url)
        // If it succeeds
        .then((response) => {
            // If the response is not good then throw an error
            if (!response.ok) {
                throw new Error(`Network response was not OK. Status: ${response.status}`);
            }
        
            // Return the json from the response if everything is good
            return response.json();
        })
        // If it tails
        .catch((error) => {
            // Throw an error
            console.error('Error fetching data:', error);
            throw error; // Rethrow the error
        });
}

// ===== | Exports | =====

export { fetchJson }
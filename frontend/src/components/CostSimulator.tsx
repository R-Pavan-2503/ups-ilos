import React, { useEffect, useState } from 'react'; // Import React

import { Bar } from 'react-chartjs-2';
import {
    Chart as ChartJS,
    CategoryScale,
    LinearScale,
    BarElement,
    Title,
    Tooltip,
    Legend,
} from 'chart.js';

// This "registers" the parts of the chart we want to use
ChartJS.register(
    CategoryScale,
    LinearScale,
    BarElement,
    Title,
    Tooltip,
    Legend
);

// Define the component as a function
const CostSimulator: React.FC = () => {
    // We'll add state and logic here later
    const [weight, setWeight] = useState('');
    const [distance, setDistance] = useState('');
    const [selectedRate, setSelectedRate] = useState('');

    // This will hold our list of rates from the API (like { id: '...', rateName: 'Ground', ... })
    const [rateOptions, setRateOptions] = useState<any[]>([]); // We'll type this better later

    // This will hold the final result
    const [totalCost, setTotalCost] = useState<number | null>(null);

    const [costBreakdown, setCostBreakdown] = useState<{
        baseRate: number;
        weightCost: number;
        distanceCost: number;
    } | null>(null);

    useEffect(() => {
        // We define an async function inside the effect
        const fetchRates = async () => {
            try {
                // Call our .NET API endpoint
                const response = await fetch('http://localhost:5032/api/Rates'); // <-- Use your port number
                const data = await response.json();
                setRateOptions(data); // Store the fetched rates in our state
            } catch (error) {
                console.error("Error fetching rates:", error);
            }
        };

        fetchRates(); // Call the function
    }, []);

    const handleSubmit = async (e: React.FormEvent) => {
        // 1. Stop the form from reloading the page
        e.preventDefault();

        // 2. Clear any old results
        setTotalCost(null);
        setCostBreakdown(null);

        // 3. Prepare the data to send (the JSON body)
        const simulationRequest = {
            weight: parseFloat(weight), // Convert string state to a number
            distance: parseFloat(distance), // Convert string state to a number
            rateName: selectedRate
        };

        // 4. Send the data to our POST endpoint
        try {
            const response = await fetch('http://localhost:5032/api/Rates/simulate', { // <-- Use your port
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify(simulationRequest), // Turn our JS object into a JSON string
            });

            if (!response.ok) {
                // Handle errors from the API (like 'Rate name not found')
                const errorData = await response.json();
                console.error("Error from API:", errorData);
                return; // Stop here
            }

            // 5. Get the result and store it in state
            const result = await response.json();
            setTotalCost(result.totalCost);

            // Add this line:
            setCostBreakdown(result);
            console.log(totalCost);

        } catch (error) {
            console.error("Error submitting simulation:", error);
        }
    };

    const options = {
        responsive: true,
        plugins: {
            legend: {
                display: false, // We don't need a legend for one dataset
            },
            title: {
                display: true,
                text: 'Cost Breakdown', // Title for the chart
            },
        },
    };

    // 2. Define chart data
    const data = {
        // These are the labels for the X-axis
        labels: ['Base Rate', 'Weight Cost', 'Distance Cost'],
        datasets: [
            {
                label: 'Cost ($)',
                // The actual data for the bars
                data: [
                    costBreakdown?.baseRate || 0,
                    costBreakdown?.weightCost || 0,
                    costBreakdown?.distanceCost || 0,
                ],
                // Bar colors
                backgroundColor: [
                    'rgba(255, 99, 132, 0.5)',
                    'rgba(54, 162, 235, 0.5)',
                    'rgba(75, 192, 192, 0.5)',
                ],
            },
        ],
    };

    return (
        <div className="container mx-auto p-4">
            <h2 className="text-2xl font-bold mb-4">Shipment Cost Simulator</h2>
            <form onSubmit={handleSubmit} className="space-y-4">                {/* Weight Input */}
                <div>
                    <label htmlFor="weight" className="block text-sm font-medium text-gray-700">
                        Weight (lbs)
                    </label>
                    <input
                        type="number"
                        id="weight"
                        value={weight}
                        onChange={(e) => setWeight(e.target.value)}
                        className="mt-1 block w-full rounded-md border-gray-300 shadow-sm focus:border-indigo-500 focus:ring-indigo-500"
                        placeholder="e.g., 10"
                    />
                </div>

                {/* Distance Input */}
                <div>
                    <label htmlFor="distance" className="block text-sm font-medium text-gray-700">
                        Distance (miles)
                    </label>
                    <input
                        type="number"
                        id="distance"
                        value={distance}
                        onChange={(e) => setDistance(e.target.value)}
                        className="mt-1 block w-full rounded-md border-gray-300 shadow-sm focus:border-indigo-500 focus:ring-indigo-500"
                        placeholder="e.g., 100"
                    />
                </div>

                {/* Rate Options Dropdown */}
                <div>
                    <label htmlFor="rate" className="block text-sm font-medium text-gray-700">
                        Shipment Mode
                    </label>
                    <select
                        id="rate"
                        value={selectedRate}
                        onChange={(e) => setSelectedRate(e.target.value)}
                        className="mt-1 block w-full rounded-md border-gray-300 shadow-sm focus:border-indigo-500 focus:ring-indigo-500"
                    >
                        <option value="">-- Select a rate --</option>
                        {rateOptions.map((rate) => (
                            <option key={rate.id} value={rate.rateName}>
                                {rate.rateName}
                            </option>
                        ))}
                    </select>
                </div>

                {/* Submit Button */}
                <div>
                    <button
                        type="submit"
                        className="w-full justify-center rounded-md border border-transparent bg-indigo-600 py-2 px-4 text-sm font-medium text-white shadow-sm hover:bg-indigo-700 focus:outline-none focus:ring-2 focus:ring-indigo-500 focus:ring-offset-2"
                    >
                        Calculate Cost
                    </button>
                </div>
            </form>
            {totalCost !== null && (
                <div className="mt-6 rounded-md bg-green-100 p-4 shadow">
                    <h3 className="text-lg font-medium text-green-800">
                        Estimated Total Cost
                    </h3>
                    <p className="mt-2 text-3xl font-bold text-green-900">
                        ${totalCost.toFixed(2)}
                    </p>
                    <div className="mt-4 h-64">
                        <Bar options={options} data={data} />
                    </div>
                </div>
            )}
        </div>
    );
};

export default CostSimulator; // Export the component
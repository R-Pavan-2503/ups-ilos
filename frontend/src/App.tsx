import CostSimulator from './components/CostSimulator'; // Import our new component

function App() {
  return (
    <div className="min-h-screen bg-gray-100">
      <header className="bg-white shadow">
        <div className="container mx-auto px-4 py-6">
          <h1 className="text-3xl font-bold text-gray-900">
            UPS ILOS Dashboard
          </h1>
        </div>
      </header>
      <main className="container mx-auto p-4">
        {/* Render our component here */}
        <CostSimulator />
      </main>
    </div>
  );
}

export default App;
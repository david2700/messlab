"use client";

import { useState } from "react";

export default function LoginPage() {
  const [username, setUsername] = useState("");
  const [password, setPassword] = useState("");
  const [message, setMessage] = useState("");


  async function handleLogin(e: React.FormEvent) {
    e.preventDefault();

    const res = await fetch("http://localhost:5005/api/auth/login", {
        method: "POST",
        headers: {
            "Content-Type": "application/json",
        },
        body: JSON.stringify({ username, password }),
    });

    const data = await res.json();
    setMessage(data.message);
  }

  return (
    <div className="flex min-h-screen items-center justify-center bg-zinc-50 font-sans">
        <form 
            onSubmit={handleLogin}
            className="flex flex-col gap-4 bg-white p-8 rounded-lg shadow-md w-80 items-center"
        >
            <h1 className="text-lg font-semibold text-black">Messlab Login</h1>

            <input
                type="text"
                value={username}
                onChange={(e) => setUsername(e.target.value)}
                placeholder="Username"
                className="border border-gray-300 p-2 rounded text-black"
            />
            <input
                type="password"
                value={password}
                onChange={(e) => setPassword(e.target.value)}
                placeholder="Password"
                className="border border-gray-300 p-2 rounded text-black"
            />
            <button
                type="submit"
                className="bg-blue-500 text-white p-2 rounded hover:bg-blue-600 cursor-pointer"
            >
                Login
            </button>
            {message && <p className="text-gray-500 text-sm text-center">{message}</p>}
        </form>
    </div>
  )
}


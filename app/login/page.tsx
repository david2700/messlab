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
        <div className="absolute top-20 left-10 text-9xl font-bold text-black/5 pointer-events-none">
            Welcome to the MessLab.
        </div>
        <form 
            onSubmit={handleLogin}
            className="flex flex-col gap-4 bg-white p-8 rounded-lg shadow-md w-80 items-center"
        >
            <h1 className="text-lg font-semibold text-black">Messlab Login</h1>
            
            <div className="flex flex-col gap-2 w-full">
                <label htmlFor="username" className="text-black text-sm" >Username</label>
                <input
                    type="text"
                    value={username}
                    onChange={(e) => setUsername(e.target.value)}
                    placeholder="Username"
                    className="border border-gray-300 p-2 rounded text-black focus:outline-black focus:border-black"
                />
            </div>
            
            <div className="flex flex-col gap-2 w-full">
                <label htmlFor="password" className="text-black text-sm" >Password</label>
                <input
                    type="password"
                    value={password}
                    onChange={(e) => setPassword(e.target.value)}
                    placeholder="Password"
                    className="border border-gray-300 p-2 rounded text-black focus:outline-black focus:border-black"
                />
            </div>
            <button
                type="submit"
                className="bg-black text-white p-2 rounded hover:bg-black/50 cursor-pointer w-full mt-4"
            >
                Login
            </button>
            {message && <p className="text-gray-500 text-sm text-center">{message}</p>}
        </form>
    </div>
  )
}


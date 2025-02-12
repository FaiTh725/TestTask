import { BrowserRouter, Route, Routes } from 'react-router-dom'
import './App.css'
import Home from './pages/Home/Home'
import NotFound from './pages/NotFound/NotFound'
import Login from './pages/Login/Login'
import Register from './pages/Register/Register'
import Event from './pages/Event/Event'
import { AuthProvider } from './components/Auth/AuthContext'
import ProtectedRoute from './components/Auth/ProtectedRoute'

function App() {

  return (
    <BrowserRouter>
      <AuthProvider>
        <Routes>
          <Route path='*' element={<NotFound/>}/>
          <Route path='/' element={<Home/>}/>
          <Route path='/account/login' element={<Login/>}/>
          <Route path='/account/register' element={<Register/>}/>
          <Route path='/event' element={<Event/>}/>
          <Route element={<ProtectedRoute roles={["Admin"]}/>}>
          </Route>
        </Routes>
      </AuthProvider>
    </BrowserRouter>
  )
}

export default App

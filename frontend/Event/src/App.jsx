import { BrowserRouter, Route, Routes } from 'react-router-dom'
import './App.css'
import Home from './pages/Home/Home'
import NotFound from './pages/NotFound/NotFound'
import Login from './pages/Login/Login'
import Register from './pages/Register/Register'
import Event from './pages/Event/Event'

function App() {

  return (
    <BrowserRouter>
      <Routes>
        <Route path='*' element={<NotFound/>}/>
        <Route path='/' element={<Home/>}/>
        <Route path='/account/login' element={<Login/>}/>
        <Route path='/account/register' element={<Register/>}/>
        <Route path='/event' element={<Event/>}/>
      </Routes>
    </BrowserRouter>
  )
}

export default App

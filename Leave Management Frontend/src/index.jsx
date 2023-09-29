import React from 'react'
import { createRoot } from 'react-dom/client'
import LeaveManagementApp from './LeaveManagementApp'
import './index.css'

document.addEventListener('DOMContentLoaded', function() {
  createRoot(document.body.appendChild(document.createElement('div')))
    .render(<LeaveManagementApp />)
})

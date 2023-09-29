import React, { lazy, Suspense } from 'react'
import { createEventId } from './event-utils'

const LazyCalendar = lazy(() => import('./Calendar'))
const LazyPlaceholder = lazy(() => import('./Placeholder'))
const LazyPopup = lazy(() => import('./LazyPopup'))

export default class LeaveManagementApp extends React.Component {

  state = {
    calendarVisible: true,
    weekendsVisible: true,
    popupVisible: false,
    currentEvents: []
  }

  render() {
    return (
      <div className='app'>
        {this.renderHeader()}
        {this.renderSidebar()}
        <div className='app-main'>
          <Suspense>
            {!this.state.calendarVisible ? (
              <LazyPlaceholder />
            )
             : (
              <LazyCalendar
                // initialEvents={{INITIAL_EVENTS}}
                weekendsVisible={this.state.weekendsVisible}
                onDateSelect={this.handleDateSelect}
                onEventClick={this.handleEventClick}
                onEvents={this.handleEvents}
              />
            )}
            {this.state.popupVisible ? (
              <LazyPopup />
            ): null}
          </Suspense>
        </div>
      </div>
    )
  }

  renderSidebar() {
    return (
      <div className='app-sidebar'>
        <div className='app-sidebar-section'>
          <h2>Instructions</h2>
          <ul>
            <li>For all-day leave, use the month view to select the days you'd like to submit for leave</li>
            <li>For partial leave-days, you can use the weekly, or daily view to specify the exact times of your leave</li>
            <li>You can drag and drop your events to make adjustments, or click your own leave days to delete the submission.</li>
          </ul>
        </div>
        {/* <div className='app-sidebar-section'>
          <label>
            <input
              type='checkbox'
              checked={this.state.calendarVisible}
              onChange={this.handleToggleCalendar}
            ></input>
            toggle calendar
          </label>
        </div> */}
        <div className='app-sidebar-section'>
          <label>
            <input
              type='checkbox'
              checked={this.state.weekendsVisible}
              onChange={this.handleToggleWeekends}
            ></input>
            toggle weekends
          </label>
        </div>
        <div className='app-sidebar-section'>
          <label>
            <input
              type='checkbox'
              checked={this.state.popupVisible}
              onChange={this.handleTogglePopup}
            ></input>
            Popup Visible
          </label>
        </div>
        {/* <div className='app-sidebar-section'>
          <h2>All Events ({this.state.currentEvents.length})</h2>
          <ul>
            {this.state.currentEvents.map(renderSidebarEvent)}
          </ul>
        </div> */}
      </div>
    )
  }

  renderHeader() {

    return (
      <div className='app-header'>
        <div className='app-header-section'>
          <h1>Leave Management Service</h1>
        </div>
      </div>
    )
  }

  handleToggleCalendar = () => {
    this.setState({
      calendarVisible: !this.state.calendarVisible
    })
  }

  handleTogglePopup = () => {
    this.setState({
      popupVisible: !this.state.popupVisible
    })
  }

  handleToggleWeekends = () => {
    this.setState({
      weekendsVisible: !this.state.weekendsVisible
    })
  }

  handleDateSelect = (selectInfo) => {
    let title = prompt('Please enter a reason for your leave.')
    let calendarApi = selectInfo.view.calendar

    // Clear your date selection.
    calendarApi.unselect()

    //If leave reason was given.
    if (title) {
      calendarApi.addEvent({
        id: createEventId(),
        title,
        start: selectInfo.startStr,
        end: selectInfo.endStr,
        allDay: selectInfo.allDay
      })
    }
  }

  handleEventClick = (clickInfo) => {
    if (confirm(`Are you sure you want to delete the leave request '${clickInfo.event.title}'?`)) {
      clickInfo.event.remove()
    }
  }

  handleEvents = (events) => {
    this.setState({
      currentEvents: events
    })
  }

}
